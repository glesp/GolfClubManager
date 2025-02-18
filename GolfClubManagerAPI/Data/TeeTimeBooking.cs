namespace GolfClubManagerAPI.Data;

public class TeeTimeBooking
{
    public int Id { get; set; }
    public DateTime BookingTime { get; set; }
    public int MemberId { get; set; }
    public Member Member { get; set; }
}