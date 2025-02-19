namespace GolfClubManagerWASM.DTOs;

public class BookingDTO
{
    public int MemberId { get; set; }
    public int TeeTimeSlotId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}