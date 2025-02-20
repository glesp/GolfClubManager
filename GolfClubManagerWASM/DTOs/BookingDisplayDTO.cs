namespace GolfClubManagerWASM.DTOs;

public class BookingDisplayDTO
{
    public string MemberName { get; set; }
    public DateTime BookingTime { get; set; }
    public int TeeTimeSlotId { get; set; }
    public int Handicap { get; set; }
}