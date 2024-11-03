using Backend.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Data.ExtendedModel;
using Backend.Data.Models;

namespace Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Units> Units { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<HTMLData> HTMLData { get; set; }
        public DbSet<GifData> GifData { get; set; }
        public DbSet<GlassStormIoData> GlassStormIoData { get; set; }
        public DbSet<DailyHTMLReading> DailyHTMLReading { get; set; }
        public DbSet<DailyGifReading> DailyGifReading { get; set; }
        public DbSet<DailyGlassStormReading> DailyGlassStormReading { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<TrainingUnits> TrainingUnits { get; set; }
        public DbSet<UniversalReading> UniversalReading { get; set; }
        public DbSet<TrainingLog> TrainingLog { get; set; }
        public DbSet<TrainingSetsLog> TrainingSetsLog { get; set; }
        public DbSet<TrainingBlog> TrainingBlog { get; set; }
        public DbSet<SeaBlog> SeaBlog { get; set; }
        public DbSet<SeaComment> SeaComment { get; set; }
        public DbSet<TrainingComment> TrainingComment { get; set; }
        public DbSet<DataFetchingLog> DataFetchingLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locations>()
                .HasIndex(l => l.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Locations>().HasData(
                new Locations { Id = -1, Name = "Шабла" },
                new Locations { Id = -2, Name = "Калиакра" },
                new Locations { Id = -3, Name = "Варна" },
                new Locations { Id = -4, Name = "Емине" },
                new Locations { Id = -5, Name = "Бургас" },
                new Locations { Id = -6, Name = "Ахтопол" }
            );

            modelBuilder.Entity<Units>().HasData(
                new Units { UnitId = -1, UnitName = "Бала" },
                new Units { UnitId = -2, UnitName = "Метра" },
                new Units { UnitId = -3, UnitName = "°C" },
                new Units { UnitId = -4, UnitName = "м/с" }
            );
        }
    }
}

