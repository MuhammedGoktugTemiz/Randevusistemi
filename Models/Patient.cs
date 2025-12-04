using System.ComponentModel.DataAnnotations.Schema;

namespace RandevuWeb.Models;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string TcKimlik { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;
    public string Allergies { get; set; } = string.Empty;
    public string EmergencyContact { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Status { get; set; } = "Aktif";
    public string StatusColor { get; set; } = "#4ade80";
    public string LastVisit { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string ToothNumbers { get; set; } = string.Empty;
    public string ProcedureDetails { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [NotMapped]
    public int Age
    {
        get
        {
            if (BirthDate.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
            return 0;
        }
    }
}

