using System.Data.Entity;
using RandevuWeb.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RandevuWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null); // Code First migrations kullanmayacağız
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Patient configuration
            modelBuilder.Entity<Patient>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Patient>()
                .Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Patient>()
                .Property(e => e.TcKimlik)
                .HasMaxLength(11);
            modelBuilder.Entity<Patient>()
                .Property(e => e.Phone)
                .HasMaxLength(20);
            modelBuilder.Entity<Patient>()
                .Property(e => e.Email)
                .HasMaxLength(100);
            modelBuilder.Entity<Patient>()
                .Property(e => e.Address)
                .HasMaxLength(500);
            modelBuilder.Entity<Patient>()
                .Property(e => e.BloodType)
                .HasMaxLength(10);
            modelBuilder.Entity<Patient>()
                .Property(e => e.Status)
                .HasMaxLength(50);
            modelBuilder.Entity<Patient>()
                .Property(e => e.StatusColor)
                .HasMaxLength(20);
            modelBuilder.Entity<Patient>()
                .HasIndex(e => e.TcKimlik);

            // Doctor configuration
            modelBuilder.Entity<Doctor>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Doctor>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Doctor>()
                .Property(e => e.Specialty)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Doctor>()
                .Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Doctor>()
                .Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<Doctor>()
                .Property(e => e.PhoneNumber)
                .HasMaxLength(20);
            modelBuilder.Entity<Doctor>()
                .HasIndex(e => e.Username)
                .IsUnique();

            // Appointment configuration
            modelBuilder.Entity<Appointment>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Appointment>()
                .Property(e => e.PatientName)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Appointment>()
                .Property(e => e.DoctorName)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Appointment>()
                .Property(e => e.DoctorSpecialty)
                .HasMaxLength(200);
            modelBuilder.Entity<Appointment>()
                .Property(e => e.Procedure)
                .HasMaxLength(200);
            modelBuilder.Entity<Appointment>()
                .Property(e => e.ToothNumbers)
                .HasMaxLength(100);
            modelBuilder.Entity<Appointment>()
                .Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Appointment>()
                .HasIndex(e => e.PatientId);
            modelBuilder.Entity<Appointment>()
                .HasIndex(e => e.DoctorId);
            modelBuilder.Entity<Appointment>()
                .HasIndex(e => e.Start);

            // Foreign Key Relationships
            modelBuilder.Entity<Appointment>()
                .HasRequired(e => e.Patient)
                .WithMany()
                .HasForeignKey(e => e.PatientId)
                .WillCascadeOnDelete(false); // Hasta silinirse randevular korunur

            modelBuilder.Entity<Appointment>()
                .HasRequired(e => e.Doctor)
                .WithMany()
                .HasForeignKey(e => e.DoctorId)
                .WillCascadeOnDelete(false); // Doktor silinirse randevular korunur
        }
    }
}
