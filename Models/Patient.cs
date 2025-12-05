using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandevuWeb.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string TcKimlik { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BloodType { get; set; }
        public string Allergies { get; set; }
        public string EmergencyContact { get; set; }
        public string PhotoUrl { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string LastVisit { get; set; }
        public string Notes { get; set; }
        public string ToothNumbers { get; set; }
        public string ProcedureDetails { get; set; }
        public DateTime CreatedAt { get; set; }

        public Patient()
        {
            Status = "Aktif";
            StatusColor = "#4ade80";
            LastVisit = string.Empty;
            Notes = string.Empty;
            ToothNumbers = string.Empty;
            ProcedureDetails = string.Empty;
            CreatedAt = DateTime.Now;
            FullName = string.Empty;
            TcKimlik = string.Empty;
            Gender = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            BloodType = string.Empty;
            Allergies = string.Empty;
            EmergencyContact = string.Empty;
            PhotoUrl = string.Empty;
        }

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
}
