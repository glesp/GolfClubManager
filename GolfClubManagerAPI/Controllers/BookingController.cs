using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolfClubManagerAPI.Data;
using GolfClubManagerAPI.Models;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all tee time slots with their players.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await _context.TeeTimeSlots
            .Include(slot => slot.Bookings)
                .ThenInclude(booking => booking.Member) // Include player details
            .Select(slot => new
            {
                SlotId = slot.Id,
                BookingTime = slot.BookingTime,
                Players = slot.Bookings.Select(b => b.Member.Name).ToList()
            })
            .ToListAsync();

        return Ok(bookings);
    }

    /// <summary>
    /// Add a member to a tee time slot.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> BookTeeTime([FromBody] TeeTimeBooking booking)
    {
        var slot = await _context.TeeTimeSlots
            .Include(s => s.Bookings)
            .FirstOrDefaultAsync(s => s.Id == booking.TeeTimeSlotId);

        if (slot == null)
            return NotFound("Tee time slot not found.");

        if (slot.Bookings.Count >= 4)
            return BadRequest("This tee time is already full.");

        _context.TeeTimeBookings.Add(booking);
        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Cancel all bookings in a tee time slot.
    /// </summary>
    [HttpDelete("{slotId}")]
    public async Task<IActionResult> CancelBooking(int slotId)
    {
        var bookings = _context.TeeTimeBookings.Where(b => b.TeeTimeSlotId == slotId);

        if (!bookings.Any()) return NotFound();

        _context.TeeTimeBookings.RemoveRange(bookings);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
