namespace GolfClubManagerAPI.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Handicap { get; set; }

        // Navigation property to bookings
        public List<TeeTimeBooking> Bookings { get; set; } = new();
    }
}