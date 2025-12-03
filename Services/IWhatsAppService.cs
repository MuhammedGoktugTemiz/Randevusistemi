namespace RandevuWeb.Services;

public interface IWhatsAppService
{
    Task<bool> SendMessageAsync(string phoneNumber, string message);
    Task<bool> SendAppointmentCreatedAsync(RandevuWeb.Models.Appointment appointment, RandevuWeb.Models.Doctor doctor);
    Task<bool> SendAppointmentReminderAsync(RandevuWeb.Models.Appointment appointment, RandevuWeb.Models.Doctor doctor);
}

