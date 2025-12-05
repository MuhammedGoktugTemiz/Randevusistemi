using RandevuWeb.Models;

namespace RandevuWeb.Services;

public class WhatsAppReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WhatsAppReminderService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Her saat kontrol et

    public WhatsAppReminderService(IServiceProvider serviceProvider, ILogger<WhatsAppReminderService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndSendRemindersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WhatsApp hatırlatma kontrolü sırasında hata oluştu");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task CheckAndSendRemindersAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
        var whatsAppService = scope.ServiceProvider.GetRequiredService<IWhatsAppService>();

        var tomorrow = DateTime.Today.AddDays(1);
        var appointments = dataService.GetAppointments()
            .Where(a => a.Start.Date == tomorrow.Date && a.Status == "Bekliyor")
            .ToList();

        _logger.LogInformation($"Yarın için {appointments.Count} randevu bulundu. Hatırlatma mesajları kontrol ediliyor...");

        foreach (var appointment in appointments)
        {
            try
            {
                var doctor = dataService.GetDoctor(appointment.DoctorId);
                if (doctor != null && !string.IsNullOrWhiteSpace(doctor.PhoneNumber))
                {
                    // Hatırlatma gönderilmiş mi kontrol et (basit bir mekanizma - gerçek uygulamada veritabanında saklanmalı)
                    var reminderSent = await CheckReminderSentAsync(appointment.Id, tomorrow);
                    
                    if (!reminderSent)
                    {
                        var success = await whatsAppService.SendAppointmentReminderAsync(appointment, doctor);
                        if (success)
                        {
                            await MarkReminderSentAsync(appointment.Id, tomorrow);
                            _logger.LogInformation($"Hatırlatma mesajı gönderildi: {doctor.Name} - {appointment.PatientName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Randevu {appointment.Id} için hatırlatma gönderilirken hata oluştu");
            }
        }
    }

    private async Task<bool> CheckReminderSentAsync(int appointmentId, DateTime date)
    {
        // Basit dosya tabanlı kontrol (gerçek uygulamada veritabanı kullanılmalı)
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "reminders.json");
        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var reminders = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<int>>>(json) 
                ?? new Dictionary<string, List<int>>();
            
            var dateKey = date.ToString("yyyy-MM-dd");
            return reminders.ContainsKey(dateKey) && reminders[dateKey].Contains(appointmentId);
        }
        catch
        {
            return false;
        }
    }

    private async Task MarkReminderSentAsync(int appointmentId, DateTime date)
    {
        var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        var filePath = Path.Combine(dataPath, "reminders.json");
        var reminders = new Dictionary<string, List<int>>();

        if (File.Exists(filePath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                reminders = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<int>>>(json) 
                    ?? new Dictionary<string, List<int>>();
            }
            catch { }
        }

        var dateKey = date.ToString("yyyy-MM-dd");
        if (!reminders.ContainsKey(dateKey))
        {
            reminders[dateKey] = new List<int>();
        }

        if (!reminders[dateKey].Contains(appointmentId))
        {
            reminders[dateKey].Add(appointmentId);
        }

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var jsonOutput = System.Text.Json.JsonSerializer.Serialize(reminders, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, jsonOutput);
    }
}

