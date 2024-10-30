using Letovi.Web.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Letovi.Web.Data
{
    public class FlightDbContext : IdentityDbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Konfiguracija za Flight
            builder.Entity<Flight>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DepartureCity).IsRequired();
                entity.Property(e => e.ArrivalCity).IsRequired();
                entity.Property(e => e.NumberOfStops).IsRequired();
                entity.Property(e => e.SeatCount).IsRequired();
                entity.Property(e => e.AvailableSeats).IsRequired();

                // Konfiguracija za navigaciono svojstvo
                entity.HasMany(e => e.Reservations)
                      .WithOne(r => r.Flight)
                      .HasForeignKey(r => r.FlightId);
            });

            // Konfiguracija za Reservation
            builder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FlightId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.NumberOfSeats).IsRequired();
                entity.Property(e => e.IsApproved).IsRequired();
            });

            
            var adminRoleId = "a545458f-fe9e-4509-9644-078becddcc2a";
            var agentRoleId = "cc51a88b-231e-4a5a-8621-dfbfcb6f246e";
            var posetilacRoleId = "eecf777e-c3b5-452a-9235-7b8f1c0c051b";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName="Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId

                },
                new IdentityRole {
                    Name = "Agent",
                    NormalizedName = "Agent",
                    Id = agentRoleId,
                    ConcurrencyStamp = agentRoleId
                },
                new IdentityRole {
                    Name = "Posetilac",
                    NormalizedName = "Posetilac",
                    Id = posetilacRoleId,
                    ConcurrencyStamp= posetilacRoleId


                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

           
            var adminId = "61aa864b-cf91-404f-92dc-cbdc053d9e5f";
            var adminUser = new IdentityUser
            {
                UserName = "nemanjatomic98@gmail.com",
                Email = "nemanjatomic98@gmail.com",
                NormalizedEmail = "nemanjatomic98@gmail.com".ToUpper(),
                NormalizedUserName = "nemanjatomic98@gmail.com".ToUpper(),
                Id = adminId

            };
            adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "nemanja1234");
            builder.Entity<IdentityUser>().HasData(adminUser);

            // dodavanje svih rolova adminu
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId=adminRoleId ,
                    UserId = adminId
                },
                new IdentityUserRole<string>
                {
                    RoleId= agentRoleId,
                    UserId = adminId
                },
                new IdentityUserRole<string>
                {
                    RoleId= posetilacRoleId,
                    UserId = adminId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
