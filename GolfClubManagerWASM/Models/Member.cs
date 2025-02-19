namespace GolfClubManagerWASM.Models;

public class Member
{
    public int Id { get; set; }
    public int MembershipNumber { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public int Handicap { get; set; }
}