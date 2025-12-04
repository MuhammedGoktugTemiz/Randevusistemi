using Microsoft.EntityFrameworkCore;
using RandevuWeb.Models;

namespace RandevuWeb.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Patient configuration
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TcKimlik).HasMaxLength(11);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.BloodType).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StatusColor).HasMaxLength(20);
            entity.HasIndex(e => e.TcKimlik);
        });

        // Doctor configuration
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Specialty).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(500);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // Appointment configuration
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PatientName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DoctorName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DoctorSpecialty).HasMaxLength(200);
            entity.Property(e => e.Procedure).HasMaxLength(200);
            entity.Property(e => e.ToothNumbers).HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.PatientId);
            entity.HasIndex(e => e.DoctorId);
            entity.HasIndex(e => e.Start);

            // Foreign Key Relationships
            entity.HasOne(e => e.Patient)
                .WithMany()
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Hasta silinirse randevular korunur

            entity.HasOne(e => e.Doctor)
                .WithMany()
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Doktor silinirse randevular korunur
        });
    }
}

