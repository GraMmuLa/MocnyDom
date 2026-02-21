using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using MocnyDom.Domain.Entities;

namespace MocnyDom.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BuildingManager> BuildingManagers { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("MocnyDom.Infrastructure"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Building -> Floors
            builder.Entity<Building>()
                .HasMany(b => b.Floors)
                .WithOne(f => f.Building)
                .HasForeignKey(f => f.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Floor -> Rooms
            builder.Entity<Floor>()
                .HasMany(f => f.Rooms)
                .WithOne(r => r.Floor)
                .HasForeignKey(r => r.FloorId)
                .OnDelete(DeleteBehavior.Cascade);

            // BuildingManager -> IdentityUser
            builder.Entity<BuildingManager>()
    .HasKey(x => new { x.UserId, x.BuildingId });

            // BuildingManager -> Building
            builder.Entity<BuildingManager>()
                .HasOne(bm => bm.Building)
                .WithMany(b => b.Managers)
                .HasForeignKey(bm => bm.BuildingId);
        }
    }
}
