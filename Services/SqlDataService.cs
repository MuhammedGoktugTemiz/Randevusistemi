using System;
using System.Collections.Generic;
using System.Linq;
using RandevuWeb.Data;
using RandevuWeb.Models;

namespace RandevuWeb.Services
{
    public class SqlDataService : IDataService
    {
        private readonly ApplicationDbContext _context;

        public SqlDataService()
        {
            _context = new ApplicationDbContext();
        }

        #region Patients

        public List<Patient> GetPatients()
        {
            return _context.Patients
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

        public Patient GetPatient(int id)
        {
            return _context.Patients.Find(id);
        }

        public void SavePatient(Patient patient)
        {
            if (patient.Id == 0)
            {
                // Yeni hasta
                if (!_context.Patients.Any())
                {
                    patient.Id = 1;
                }
                else
                {
                    patient.Id = _context.Patients.Max(p => p.Id) + 1;
                }
                patient.CreatedAt = DateTime.Now;
                _context.Patients.Add(patient);
            }
            else
            {
                // Mevcut hasta güncelleme
                var existing = _context.Patients.Find(patient.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(patient);
                }
            }
            _context.SaveChanges();
        }

        public void DeletePatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }

        #endregion

        #region Doctors

        public List<Doctor> GetDoctors()
        {
            return _context.Doctors
                .OrderBy(d => d.Name)
                .ToList();
        }

        public Doctor GetDoctor(int id)
        {
            return _context.Doctors.Find(id);
        }

        public void SaveDoctor(Doctor doctor)
        {
            if (doctor.Id == 0)
            {
                // Yeni doktor
                if (!_context.Doctors.Any())
                {
                    doctor.Id = 1;
                }
                else
                {
                    doctor.Id = _context.Doctors.Max(d => d.Id) + 1;
                }
                _context.Doctors.Add(doctor);
            }
            else
            {
                // Mevcut doktor güncelleme
                var existing = _context.Doctors.Find(doctor.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(doctor);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteDoctor(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                _context.SaveChanges();
            }
        }

        public Doctor GetDoctorByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return _context.Doctors
                .FirstOrDefault(d => d.Username.Trim().Equals(username.Trim(), StringComparison.Ordinal));
        }

        #endregion

        #region Appointments

        public List<Appointment> GetAppointments()
        {
            return _context.Appointments
                .OrderBy(a => a.Start)
                .ToList();
        }

        public Appointment GetAppointment(int id)
        {
            return _context.Appointments.Find(id);
        }

        public void SaveAppointment(Appointment appointment)
        {
            if (appointment.Id == 0)
            {
                // Yeni randevu
                if (!_context.Appointments.Any())
                {
                    appointment.Id = 1;
                }
                else
                {
                    appointment.Id = _context.Appointments.Max(a => a.Id) + 1;
                }
                appointment.CreatedAt = DateTime.Now;
                _context.Appointments.Add(appointment);
            }
            else
            {
                // Mevcut randevu güncelleme
                var existing = _context.Appointments.Find(appointment.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(appointment);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _context.Appointments
                .Where(a => a.Start.Date == date.Date)
                .OrderBy(a => a.Start)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByDoctorAndDate(int doctorId, DateTime date)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.Start.Date == date.Date)
                .OrderBy(a => a.Start)
                .ToList();
        }

        public bool HasDoctorConflict(int doctorId, DateTime start, int durationMinutes, int? excludeAppointmentId = null)
        {
            var end = start.AddMinutes(durationMinutes);
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId && (excludeAppointmentId == null || a.Id != excludeAppointmentId.Value))
                .Any(a => a.Start < end && a.End > start);
        }

        #endregion
    }
}
