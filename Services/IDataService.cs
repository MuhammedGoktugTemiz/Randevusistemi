using System;
using System.Collections.Generic;
using RandevuWeb.Models;

namespace RandevuWeb.Services
{
    public interface IDataService
    {
        List<Patient> GetPatients();
        Patient GetPatient(int id);
        void SavePatient(Patient patient);
        void DeletePatient(int id);

        List<Doctor> GetDoctors();
        Doctor GetDoctor(int id);
        Doctor GetDoctorByUsername(string username);
        void SaveDoctor(Doctor doctor);
        void DeleteDoctor(int id);

        List<Appointment> GetAppointments();
        Appointment GetAppointment(int id);
        void SaveAppointment(Appointment appointment);
        void DeleteAppointment(int id);
        List<Appointment> GetAppointmentsByDate(DateTime date);
        List<Appointment> GetAppointmentsByDoctorAndDate(int doctorId, DateTime date);
        bool HasDoctorConflict(int doctorId, DateTime start, int durationMinutes, int? excludeAppointmentId = null);
    }
}
