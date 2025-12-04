using Microsoft.EntityFrameworkCore;
using RandevuWeb.Data;
using RandevuWeb.Models;
using System.Text.Json;

namespace RandevuWeb.Services;

/// <summary>
/// JSON dosyalarından SQL veritabanına veri taşıma servisi
/// </summary>
public class MigrateJsonToSql
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public MigrateJsonToSql(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task MigrateAsync()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        // Doctors migration
        var doctorsJsonPath = Path.Combine(_env.ContentRootPath, "Data", "doctors.json");
        if (File.Exists(doctorsJsonPath))
        {
            var json = await File.ReadAllTextAsync(doctorsJsonPath);
            var doctors = JsonSerializer.Deserialize<List<Doctor>>(json, jsonOptions) ?? new List<Doctor>();
            
            foreach (var doctor in doctors)
            {
                if (!_context.Doctors.Any(d => d.Id == doctor.Id))
                {
                    _context.Doctors.Add(doctor);
                }
            }
        }

        // Patients migration
        var patientsJsonPath = Path.Combine(_env.ContentRootPath, "Data", "patients.json");
        if (File.Exists(patientsJsonPath))
        {
            var json = await File.ReadAllTextAsync(patientsJsonPath);
            var patients = JsonSerializer.Deserialize<List<Patient>>(json, jsonOptions) ?? new List<Patient>();
            
            foreach (var patient in patients)
            {
                if (!_context.Patients.Any(p => p.Id == patient.Id))
                {
                    _context.Patients.Add(patient);
                }
            }
        }

        // Appointments migration
        var appointmentsJsonPath = Path.Combine(_env.ContentRootPath, "Data", "appointments.json");
        if (File.Exists(appointmentsJsonPath))
        {
            var json = await File.ReadAllTextAsync(appointmentsJsonPath);
            var appointments = JsonSerializer.Deserialize<List<Appointment>>(json, jsonOptions) ?? new List<Appointment>();
            
            foreach (var appointment in appointments)
            {
                if (!_context.Appointments.Any(a => a.Id == appointment.Id))
                {
                    _context.Appointments.Add(appointment);
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}

