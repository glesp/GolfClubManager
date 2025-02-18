using Microsoft.EntityFrameworkCore;
using GolfClubManagerAPI.Models;

namespace GolfClubManagerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<TeeTimeSlot> TeeTimeSlots { get; set; }
        public DbSet<TeeTimeBooking> TeeTimeBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraint to ensure a member can only book once per day
            modelBuilder.Entity<TeeTimeBooking>()
                .HasIndex(b => new { b.MemberId, b.TeeTimeSlotId })
                .IsUnique();
        }
    }
}