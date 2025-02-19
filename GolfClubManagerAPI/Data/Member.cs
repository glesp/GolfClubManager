using GolfClubManagerAPI.Models;

public class Member
{
    public int Id { get; set; }

    public int MembershipNumber { get; set; }  // Integer for auto-increment, no identity attribute

    public string Name { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public int Handicap { get; set; }

    public List<TeeTimeBooking> Bookings { get; set; } = new();
}