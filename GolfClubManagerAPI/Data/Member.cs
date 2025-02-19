using System.ComponentModel.DataAnnotations.Schema;

namespace GolfClubManagerAPI.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Handicap { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // This makes the MembershipNumber auto-increment
        public string MembershipNumber { get; set; } = string.Empty;
        // Navigation property to bookings
        public List<TeeTimeBooking> Bookings { get; set; } = new();
    }
}