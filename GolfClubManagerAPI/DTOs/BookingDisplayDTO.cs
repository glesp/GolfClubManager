// Update this in both your API and WASM projects
public class BookingDisplayDTO
{
    public int TeeTimeSlotId { get; set; }
    public DateTime BookingTime { get; set; }
    public int MemberId { get; set; }
    public required string MemberName { get; set; }
    public decimal Handicap { get; set; }
    public bool IsSelectedMember { get; set; } = false; // New property to highlight selected member
}