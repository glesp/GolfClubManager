using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolfClubManagerAPI.Data;

namespace GolfClubManagerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeeTimeBooking>>> GetBookings()
    {
        return await _context.TeeTimeBookings.ToListAsync();
    }

    // GET: api/bookings/available
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableTeeTimes()
    {
        var allTimes = Enumerable.Range(0, 24 * 4)
            .Select(i => DateTime.Today.AddMinutes(i * 15)) // Generate 15-min slots
            .Where(t => !_context.TeeTimeBookings.Any(b => b.BookingTime == t))
            .ToList();

        return Ok(allTimes);
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> CreateBooking(TeeTimeBooking booking)
    {
        // Check if the member already has a booking for the day
        var hasBooking = await _context.TeeTimeBookings
            .AnyAsync(b => b.MemberId == booking.MemberId && b.BookingTime.Date == booking.BookingTime.Date);

        if (hasBooking)
        {
            return BadRequest("Member already has a booking for this day.");
        }

        // Check if there are already 4 players for this slot
        var count = await _context.TeeTimeBookings
            .CountAsync(b => b.BookingTime == booking.BookingTime);

        if (count >= 4)
        {
            return BadRequest("This time slot is fully booked.");
        }

        _context.TeeTimeBookings.Add(booking);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, booking);
    }

    // DELETE: api/bookings/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking = await _context.TeeTimeBookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        _context.TeeTimeBookings.Remove(booking);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
