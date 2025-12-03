using RandevuWeb.Models;
using System.Text.Json;

namespace RandevuWeb.Services;

public class JsonDataService : IDataService
{
    private readonly string _dataPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonDataService(IWebHostEnvironment env)
    {
        _dataPath = Path.Combine(env.ContentRootPath, "Data");
        if (!Directory.Exists(_dataPath))
        {
            Directory.CreateDirectory(_dataPath);
        }

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = false
        };
    }

    #region Patients

    public List<Patient> GetPatients()
    {
        var filePath = Path.Combine(_dataPath, "patients.json");
        if (!File.Exists(filePath))
        {
            SavePatients(new List<Patient>());
            return new List<Patient>();
        }

        var json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            SavePatients(new List<Patient>());
            return new List<Patient>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Patient>>(json, _jsonOptions) ?? new List<Patient>();
        }
        catch
        {
            return new List<Patient>();
        }
    }

    public Patient? GetPatient(int id)
    {
        return GetPatients().FirstOrDefault(p => p.Id == id);
    }

    public void SavePatient(Patient patient)
    {
        var patients = GetPatients();
        var existing = patients.FirstOrDefault(p => p.Id == patient.Id);
        
        if (existing != null)
        {
            var index = patients.IndexOf(existing);
            patients[index] = patient;
        }
        else
        {
            if (patients.Any())
            {
                patient.Id = patients.Max(p => p.Id) + 1;
            }
            else
            {
                patient.Id = 1;
            }
            patients.Add(patient);
        }

        SavePatients(patients);
    }

    public void DeletePatient(int id)
    {
        var patients = GetPatients();
        patients.RemoveAll(p => p.Id == id);
        SavePatients(patients);
    }

    private void SavePatients(List<Patient> patients)
    {
        var filePath = Path.Combine(_dataPath, "patients.json");
        var json = JsonSerializer.Serialize(patients, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    #endregion

    #region Doctors

    public List<Doctor> GetDoctors()
    {
        var filePath = Path.Combine(_dataPath, "doctors.json");
        var defaults = GetDefaultDoctors();

        if (!File.Exists(filePath))
        {
            SaveDoctors(defaults);
            return defaults;
        }

        var json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            SaveDoctors(defaults);
            return defaults;
        }

        try
        {
            var doctors = JsonSerializer.Deserialize<List<Doctor>>(json, _jsonOptions) ?? new List<Doctor>();
            
            // Ensure all doctors have username and password from defaults
            bool needsUpdate = false;
            foreach (var doctor in doctors)
            {
                var defaultDoctor = defaults.FirstOrDefault(d => d.Id == doctor.Id || d.Name == doctor.Name);
                if (defaultDoctor != null)
                {
                    if (string.IsNullOrWhiteSpace(doctor.Username))
                    {
                        doctor.Username = defaultDoctor.Username;
                        needsUpdate = true;
                    }
                    if (string.IsNullOrWhiteSpace(doctor.Password))
                    {
                        doctor.Password = defaultDoctor.Password;
                        needsUpdate = true;
                    }
                }
            }
            
            // If no doctors found, use defaults
            if (doctors.Count == 0)
            {
                SaveDoctors(defaults);
                return defaults;
            }
            
            // Merge defaults for missing doctors
            foreach (var defaultDoctor in defaults)
            {
                if (!doctors.Any(d => d.Id == defaultDoctor.Id))
                {
                    doctors.Add(defaultDoctor);
                    needsUpdate = true;
                }
            }
            
            if (needsUpdate)
            {
                SaveDoctors(doctors);
            }

            return doctors;
        }
        catch
        {
            // On error, return defaults
            SaveDoctors(defaults);
            return defaults;
        }
    }

    public Doctor? GetDoctor(int id)
    {
        return GetDoctors().FirstOrDefault(d => d.Id == id);
    }

    public void SaveDoctor(Doctor doctor)
    {
        var doctors = GetDoctors();
        var existing = doctors.FirstOrDefault(d => d.Id == doctor.Id);
        
        if (existing != null)
        {
            var index = doctors.IndexOf(existing);
            doctors[index] = doctor;
        }
        else
        {
            if (doctors.Any())
            {
                doctor.Id = doctors.Max(d => d.Id) + 1;
            }
            else
            {
                doctor.Id = 1;
            }
            doctors.Add(doctor);
        }

        SaveDoctors(doctors);
    }

    public void DeleteDoctor(int id)
    {
        var doctors = GetDoctors();
        doctors.RemoveAll(d => d.Id == id);
        SaveDoctors(doctors);
    }

    private void SaveDoctors(List<Doctor> doctors)
    {
        var filePath = Path.Combine(_dataPath, "doctors.json");
        var json = JsonSerializer.Serialize(doctors, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    private List<Doctor> GetDefaultDoctors()
    {
        return new List<Doctor>
        {
            new Doctor { Id = 1, Name = "Dt. Ömer Albayrak", Specialty = "Ağız ve Diş Sağlığı", Username = "omer.albayrak", Password = "omer123", PhoneNumber = "" },
            new Doctor { Id = 2, Name = "Dr. Soner Sağaltıcı", Specialty = "Ağız Diş Ve Çene Cerrahisi", Username = "soner.sagaltici", Password = "soner123", PhoneNumber = "" },
            new Doctor { Id = 3, Name = "Dr. Büşra Kibar", Specialty = "Ortodonti Uzmanı", Username = "busra.kibar", Password = "busra123", PhoneNumber = "" }
        };
    }

    public Doctor? GetDoctorByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;
            
        var doctors = GetDoctors();
        var trimmedUsername = username.Trim();
        
        // Try exact match first (case-sensitive)
        var doctor = doctors.FirstOrDefault(d => 
            !string.IsNullOrWhiteSpace(d.Username) && 
            d.Username.Trim().Equals(trimmedUsername, StringComparison.Ordinal));
        
        // If not found, try case-insensitive
        if (doctor == null)
        {
            doctor = doctors.FirstOrDefault(d => 
                !string.IsNullOrWhiteSpace(d.Username) && 
                d.Username.Trim().Equals(trimmedUsername, StringComparison.OrdinalIgnoreCase));
        }
        
        return doctor;
    }

    #endregion

    #region Appointments

    public List<Appointment> GetAppointments()
    {
        var filePath = Path.Combine(_dataPath, "appointments.json");
        if (!File.Exists(filePath))
        {
            return new List<Appointment>();
        }

        var json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Appointment>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Appointment>>(json, _jsonOptions) ?? new List<Appointment>();
        }
        catch
        {
            return new List<Appointment>();
        }
    }

    public Appointment? GetAppointment(int id)
    {
        return GetAppointments().FirstOrDefault(a => a.Id == id);
    }

    public void SaveAppointment(Appointment appointment)
    {
        var appointments = GetAppointments();
        var existing = appointments.FirstOrDefault(a => a.Id == appointment.Id);
        
        if (existing != null)
        {
            var index = appointments.IndexOf(existing);
            appointments[index] = appointment;
        }
        else
        {
            if (appointments.Any())
            {
                appointment.Id = appointments.Max(a => a.Id) + 1;
            }
            else
            {
                appointment.Id = 1;
            }
            appointments.Add(appointment);
        }

        SaveAppointments(appointments);
    }

    public void DeleteAppointment(int id)
    {
        var appointments = GetAppointments();
        appointments.RemoveAll(a => a.Id == id);
        SaveAppointments(appointments);
    }

    public List<Appointment> GetAppointmentsByDate(DateTime date)
    {
        return GetAppointments()
            .Where(a => a.Start.Date == date.Date)
            .OrderBy(a => a.Start)
            .ToList();
    }

    public List<Appointment> GetAppointmentsByDoctorAndDate(int doctorId, DateTime date)
    {
        return GetAppointments()
            .Where(a => a.DoctorId == doctorId && a.Start.Date == date.Date)
            .OrderBy(a => a.Start)
            .ToList();
    }

    public bool HasDoctorConflict(int doctorId, DateTime start, int durationMinutes, int? excludeAppointmentId = null)
    {
        var end = start.AddMinutes(durationMinutes);
        return GetAppointments()
            .Where(a => a.DoctorId == doctorId && a.Id != excludeAppointmentId)
            .Any(a => (a.Start < end && a.End > start));
    }

    private void SaveAppointments(List<Appointment> appointments)
    {
        var filePath = Path.Combine(_dataPath, "appointments.json");
        var json = JsonSerializer.Serialize(appointments, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    #endregion
}

