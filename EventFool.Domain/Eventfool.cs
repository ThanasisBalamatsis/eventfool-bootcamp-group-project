using EventFool.Domain.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EventFool.Domain
{
    public class Eventfool : IdentityDbContext<ApplicationUser>
    {

        public Eventfool()
            : base("name=EventfoolDB")
        {

        }

        public static Eventfool Create()
        {
            return new Eventfool();
        }

        public virtual DbSet<Event> Events { get; set; }
        //public new DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .HasMany(t => t.Tickets)
                .WithRequired(u => u.User);

            modelBuilder.Entity<Event>()
                .HasMany(t => t.Tickets)
                .WithRequired(e => e.Event);

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(e => e.EventsOrganised)
            .WithMany(e => e.Users)
            .Map(m => m.ToTable("eventsOrganised").MapLeftKey("UserID").MapRightKey("EventID"));

            modelBuilder.Entity<Photo>()
                .HasMany(p => p.Events)
                .WithMany(e => e.Photos)
                .Map(m => m.ToTable("Event_Photos").MapLeftKey("PhotoID").MapRightKey("EventID"));

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.UserId, t.EventId })
                .IsUnique();

            modelBuilder.Entity<Location>()
                .Property(p => p.Latitude)
                .HasPrecision(15, 13);
            modelBuilder.Entity<Location>()
                .Property(p => p.Longitude)
                .HasPrecision(15, 13);

        }
    }
}