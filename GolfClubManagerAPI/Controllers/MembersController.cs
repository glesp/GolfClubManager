using GolfClubManagerAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace GolfClubManagerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MembersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ GET: api/members
    [HttpGet]
    public IActionResult GetMembers()
    {
        var members = _context.Members.ToList();
        if (!members.Any())  // 🛑 DEBUG: Check if data exists
        {
            Console.WriteLine("❌ No members found in DB!");
        }
        return Ok(members);
    }
}