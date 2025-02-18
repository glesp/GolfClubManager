using Microsoft.EntityFrameworkCore;

namespace GolfClubManagerAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<TeeTimeBooking> TeeTimeBookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ensure MembershipNumber is unique
        modelBuilder.Entity<Member>()
            .HasIndex(m => m.MembershipNumber)
            .IsUnique();
    }
}