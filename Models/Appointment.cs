namespace RandevuWeb.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; } // Navigation property
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; } // Navigation property
    public string DoctorName { get; set; } = string.Empty;
    public string DoctorSpecialty { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string Procedure { get; set; } = string.Empty;
    public string ToothNumbers { get; set; } = string.Empty;
    public string ProcedureDetails { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Status { get; set; } = "Bekliyor";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // NotMapped: SQL'de hesaplanmış değer
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public DateTime End => Start.AddMinutes(DurationMinutes);
}

