namespace GolfClubManagerAPI.DTOs;

public class BookingDTO
{
    public int TeeTimeSlotId { get; set; }
    public List<int> MemberIds { get; set; } = new();
    public List<PlayerDTO>? NewPlayers { get; set; } = new();
}