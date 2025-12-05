using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using RandevuWeb.Models;
using Newtonsoft.Json;

namespace RandevuWeb.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _httpClient;

        public WhatsAppService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> SendMessageAsync(string phoneNumber, string message)
        {
            try
            {
                var accessToken = WebConfigurationManager.AppSettings["WhatsApp:AccessToken"];
                var phoneNumberId = WebConfigurationManager.AppSettings["WhatsApp:PhoneNumberId"];

                if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(phoneNumberId))
                {
                    System.Diagnostics.Debug.WriteLine("WhatsApp API bilgileri yapılandırılmamış. Mesaj gönderilemedi.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    System.Diagnostics.Debug.WriteLine("Doktor telefon numarası bulunamadı.");
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

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"WhatsApp mesajı başarıyla gönderildi: {cleanPhone}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"WhatsApp mesajı gönderilemedi: {response.StatusCode} - {responseContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"WhatsApp mesajı gönderilirken hata oluştu: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendAppointmentCreatedAsync(Appointment appointment, Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.PhoneNumber))
            {
                System.Diagnostics.Debug.WriteLine($"Doktor {doctor.Name} için telefon numarası bulunamadı.");
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
                System.Diagnostics.Debug.WriteLine($"Doktor {doctor.Name} için telefon numarası bulunamadı.");
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
}

