namespace GolfClubManagerAPI.Data;

public class Member
{
    public int Id { get; set; }
    public string MembershipNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Gender { get; set; } = "Other";
    public int Handicap { get; set; }
}