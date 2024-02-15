using Homies.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Type = Homies.Data.Models.Type;

namespace Homies.Data
{
    public class HomiesDbContext : IdentityDbContext
    {
        public HomiesDbContext(DbContextOptions<HomiesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<EventParticipant> EventsParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventParticipant>()
                .HasKey(pk => new { pk.HelperId, pk.EventId });

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventsParticipants)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Type>()
                .HasData(new Type()
                {
                    Id = 1,
                    Name = "Animals"
                },
                new Type()
                {
                    Id = 2,
                    Name = "Fun"
                },
                new Type()
                {
                    Id = 3,
                    Name = "Discussion"
                },
                new Type()
                {
                    Id = 4,
                    Name = "Work"
                });

            modelBuilder
                .Entity<Event>()
                .HasData(new Event()
                {
                    Id = 1,
                    Name = "Seeded Event",
                    Description = "Seeded event via a EF Core migration.",
                    OrganiserId = "9a080fb7-aa06-4a2b-823a-fcfb7dc091ee",
                    CreatedOn = DateTime.Now.AddDays(-5),
                    Start = DateTime.Now,
                    End = DateTime.Now.AddHours(34),
                    TypeId = 2
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}