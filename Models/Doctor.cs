using System.ComponentModel.DataAnnotations.Schema;

namespace RandevuWeb.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    [NotMapped]
    public string DisplayName => $"{Name} - {Specialty}";
}

