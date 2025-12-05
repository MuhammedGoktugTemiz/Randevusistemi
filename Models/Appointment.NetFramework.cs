using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandevuWeb.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } // Navigation property
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } // Navigation property
        public string DoctorName { get; set; }
        public string DoctorSpecialty { get; set; }
        public DateTime Start { get; set; }
        public int DurationMinutes { get; set; }
        public string Procedure { get; set; }
        public string ToothNumbers { get; set; }
        public string ProcedureDetails { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Appointment()
        {
            DurationMinutes = 30;
            Status = "Bekliyor";
            CreatedAt = DateTime.Now;
            PatientName = string.Empty;
            DoctorName = string.Empty;
            DoctorSpecialty = string.Empty;
            Procedure = string.Empty;
            ToothNumbers = string.Empty;
            ProcedureDetails = string.Empty;
            Notes = string.Empty;
        }

        [NotMapped]
        public DateTime End
        {
            get { return Start.AddMinutes(DurationMinutes); }
        }
    }
}

