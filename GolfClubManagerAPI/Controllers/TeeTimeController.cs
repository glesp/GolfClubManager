using Microsoft.AspNetCore.Mvc;
using GolfClubManagerAPI.DTOs;
using GolfClubManagerAPI.Data;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class TeeTimeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TeeTimeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetTeeTimeSlots/{date}")]
    public async Task<ActionResult<IEnumerable<TeeTimeSlotDTO>>> GetTeeTimeSlots(DateTime date)
    {
        var slots = await _context.TeeTimeSlots
            .Where(slot => slot.BookingTime.Date == date.Date)
            .Select(slot => new TeeTimeSlotDTO
            {
                Id = slot.Id,
                BookingTime = slot.BookingTime
            })
            .ToListAsync();

        return Ok(slots);
    }
}