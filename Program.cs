using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RandevuWeb.Data;
using RandevuWeb.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Database - Try multiple connection strings if first fails
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    // Try hosting connection string
    connectionString = builder.Configuration.GetConnectionString("DefaultConnectionHosting");
}
if (string.IsNullOrEmpty(connectionString))
{
    // Try SQLEXPRESS instance
    connectionString = builder.Configuration.GetConnectionString("DefaultConnectionSQLEXPRESS");
}
if (string.IsNullOrEmpty(connectionString))
{
    // Try LocalDB as fallback
    connectionString = builder.Configuration.GetConnectionString("DefaultConnectionLocalDB");
}
if (string.IsNullOrEmpty(connectionString))
{
    if (builder.Environment.IsDevelopment())
    {
        throw new InvalidOperationException("No valid connection string found. Please check appsettings.json");
    }
    // Production'da uygulama başlasın ama veritabanı işlemleri çalışmayacak
    // Logger henüz hazır değil, bu yüzden geçici bir connection string kullanıyoruz
    connectionString = "Server=.;Database=temp;Trusted_Connection=True;";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    }));

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true; // XSS koruması
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // HTTPS kullanımı
        options.Cookie.SameSite = SameSiteMode.Strict; // CSRF koruması
        options.Cookie.Name = "RandevuWeb.Auth";
    });

// Services
builder.Services.AddScoped<IDataService, SqlDataService>();
builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddMemoryCache(); // Rate limiting için
builder.Services.AddHttpClient<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();
builder.Services.AddHostedService<WhatsAppReminderService>();

var app = builder.Build();

// Base path ayarı (alt klasör deployment için)
var basePath = builder.Configuration["BasePath"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_BASEPATH");
if (!string.IsNullOrEmpty(basePath))
{
    app.UsePathBase(basePath);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Test connection first
        logger.LogInformation("SQL Server bağlantısı test ediliyor...");
        var canConnect = false;
        try
        {
            canConnect = context.Database.CanConnect();
            logger.LogInformation("SQL Server bağlantısı başarılı.");
        }
        catch (Exception connEx)
        {
            logger.LogError(connEx, "SQL Server'a bağlanılamıyor! Lütfen şunları kontrol edin:");
            logger.LogError("1. SQL Server servisinin çalıştığından emin olun");
            logger.LogError("2. Connection string'in doğru olduğundan emin olun (appsettings.json)");
            logger.LogError("3. SQL Server instance adını kontrol edin (localhost\\SQLEXPRESS gibi)");
            logger.LogError("4. SQL Server Authentication'ın aktif olduğundan emin olun");
            logger.LogError("Bağlantı string'i: {ConnectionString}", connectionString?.Replace("Password=.*;", "Password=***;"));
            throw;
        }
        
        // Check for pending migrations
        var pendingMigrations = new List<string>();
        try
        {
            pendingMigrations = context.Database.GetPendingMigrations().ToList();
        }
        catch
        {
            // If migrations table doesn't exist, we'll use EnsureCreated
            logger.LogInformation("Migration tablosu bulunamadı, veritabanı oluşturulacak...");
        }
        
        if (pendingMigrations.Any())
        {
            logger.LogInformation("{Count} adet bekleyen migration uygulanıyor...", pendingMigrations.Count);
            context.Database.Migrate();
            logger.LogInformation("Migration'lar başarıyla uygulandı.");
        }
        else if (!canConnect)
        {
            // Database doesn't exist, create it with EnsureCreated
            logger.LogInformation("Veritabanı oluşturuluyor...");
            context.Database.EnsureCreated();
            logger.LogInformation("Veritabanı başarıyla oluşturuldu.");
        }
        else
        {
            logger.LogInformation("Veritabanı güncel.");
        }
        
        // Eğer veritabanı boşsa ve JSON dosyaları varsa, verileri migrate et
        if (!context.Doctors.Any() && !context.Patients.Any() && !context.Appointments.Any())
        {
            logger.LogInformation("JSON dosyalarından veri taşınıyor...");
            var migrator = new MigrateJsonToSql(context, app.Services.GetRequiredService<IWebHostEnvironment>());
            await migrator.MigrateAsync();
            logger.LogInformation("Veri taşıma tamamlandı.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Veritabanı başlatılırken hata oluştu: {Message}", ex.Message);
        logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
        
        // Production'da uygulamanın başlamasına izin ver (veritabanı sonradan düzeltilebilir)
        if (app.Environment.IsDevelopment())
        {
            throw; // Development'ta hata fırlat
        }
        else
        {
            logger.LogWarning("Production modunda uygulama veritabanı hatası ile başlatılıyor. Lütfen veritabanı bağlantısını kontrol edin.");
        }
    }
}

app.Run();

