
namespace GolfClubManagerAPI.Data
{
    public class Member
    {
        public int Id { get; set; }

        public int MembershipNumber { get; set; } // Integer for auto-increment, no identity attribute

        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Gender { get; set; }
        public int Handicap { get; set; }

        public List<TeeTimeBooking> Bookings { get; set; } = new();
    }
}