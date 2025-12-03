using RandevuWeb.Models;
using System.Text;
using System.Text.Json;

namespace RandevuWeb.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<WhatsAppService> _logger;
    private readonly HttpClient _httpClient;

    public WhatsAppService(IConfiguration configuration, ILogger<WhatsAppService> logger, HttpClient httpClient)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<bool> SendMessageAsync(string phoneNumber, string message)
    {
        try
        {
            var accessToken = _configuration["WhatsApp:AccessToken"];
            var phoneNumberId = _configuration["WhatsApp:PhoneNumberId"];

            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(phoneNumberId))
            {
                _logger.LogWarning("WhatsApp API bilgileri yapılandırılmamış. Mesaj gönderilemedi.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                _logger.LogWarning("Doktor telefon numarası bulunamadı.");
                return false;
            }

            // Telefon numarasını temizle (sadece rakamlar)
            var cleanPhone = new string(phoneNumber.Where(char.IsDigit).ToArray());
            
            // Ülke kodu yoksa ekle (Türkiye için +90)
            if (!cleanPhone.StartsWith("90") && cleanPhone.Length == 10)
            {
                cleanPhone = "90" + cleanPhone;
            }
            else if (!cleanPhone.StartsWith("90"))
            {
                cleanPhone = "90" + cleanPhone.TrimStart('0');
            }

            var url = $"https://graph.facebook.com/v18.0/{phoneNumberId}/messages";

            var payload = new
            {
                messaging_product = "whatsapp",
                to = cleanPhone,
                type = "text",
                text = new
                {
                    body = message
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"WhatsApp mesajı başarıyla gönderildi: {cleanPhone}");
                return true;
            }
            else
            {
                _logger.LogError($"WhatsApp mesajı gönderilemedi: {response.StatusCode} - {responseContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WhatsApp mesajı gönderilirken hata oluştu");
            return false;
        }
    }

    public async Task<bool> SendAppointmentCreatedAsync(Appointment appointment, Doctor doctor)
    {
        if (string.IsNullOrWhiteSpace(doctor.PhoneNumber))
        {
            _logger.LogWarning($"Doktor {doctor.Name} için telefon numarası bulunamadı.");
            return false;
        }

        var dateStr = appointment.Start.ToString("dd MMMM", new System.Globalization.CultureInfo("tr-TR"));
        var doctorLastName = doctor.Name.Split(' ').LastOrDefault() ?? "Doktor";
        
        var message = $"{doctorLastName} bey {dateStr} günü {appointment.PatientName} adı soyadı adlı hastanın randevusu tarafınıza oluşturulmuştur.";

        return await SendMessageAsync(doctor.PhoneNumber, message);
    }

    public async Task<bool> SendAppointmentReminderAsync(Appointment appointment, Doctor doctor)
    {
        if (string.IsNullOrWhiteSpace(doctor.PhoneNumber))
        {
            _logger.LogWarning($"Doktor {doctor.Name} için telefon numarası bulunamadı.");
            return false;
        }

        var dateStr = appointment.Start.ToString("dd MMMM", new System.Globalization.CultureInfo("tr-TR"));
        var timeStr = appointment.Start.ToString("HH:mm", new System.Globalization.CultureInfo("tr-TR"));
        var hour = appointment.Start.Hour;
        var doctorLastName = doctor.Name.Split(' ').LastOrDefault() ?? "Doktor";
        
        var message = $"Hatırlatma: {doctorLastName} bey yarın {dateStr} saat {hour} de {appointment.PatientName} hastaya ait işleminiz bulunmaktadır.";

        return await SendMessageAsync(doctor.PhoneNumber, message);
    }
}

