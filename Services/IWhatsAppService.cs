using System.Threading.Tasks;
using RandevuWeb.Models;

namespace RandevuWeb.Services
{
    public interface IWhatsAppService
    {
        Task<bool> SendMessageAsync(string phoneNumber, string message);
        Task<bool> SendAppointmentCreatedAsync(Appointment appointment, Doctor doctor);
        Task<bool> SendAppointmentReminderAsync(Appointment appointment, Doctor doctor);
    }
}
