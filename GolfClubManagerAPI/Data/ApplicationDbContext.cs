using Microsoft.EntityFrameworkCore;

namespace GolfClubManagerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<TeeTimeSlot> TeeTimeSlots { get; set; }
        public DbSet<TeeTimeBooking> TeeTimeBookings { get; set; }

        // In ApplicationDbContext.cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
    
            // Membership Number Fix
            modelBuilder.Entity<Member>()
                .Property(m => m.MembershipNumber)
                .IsRequired()
                .HasColumnType("int");
        
            // Other configurations...
        }
    }
}