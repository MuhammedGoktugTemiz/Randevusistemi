using System.ComponentModel.DataAnnotations.Schema;

namespace RandevuWeb.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get { return $"{Name} - {Specialty}"; }
        }
    }
}

