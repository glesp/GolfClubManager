namespace GolfClubManagerAPI.DTOs;

public class MemberDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Handicap { get; set; }
}