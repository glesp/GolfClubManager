namespace GolfClubManagerWASM.Models;

public class Member
{
    public int Id { get; set; }
    public int MembershipNumber { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Gender { get; set; }
    public int Handicap { get; set; }
}