namespace GolfClubManagerWASM.Models;

public class TeeTimeBooking
{
    public int Id { get; set; }
    public DateTime BookingTime { get; set; }
    public int MemberId { get; set; }
}