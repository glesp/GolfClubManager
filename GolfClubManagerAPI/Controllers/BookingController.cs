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
    [HttpPost]
    public async Task<IActionResult> BookTeeTime(TeeTimeBooking booking)
    {
        // Ensure the Member has not already booked a slot on the same day
        bool hasExistingBooking = await _context.TeeTimeBookings
            .Include(b => b.TeeTimeSlot) // Join with TeeTimeSlots
            .AnyAsync(b => b.MemberId == booking.MemberId &&
                           b.TeeTimeSlot.BookingTime.Date == booking.TeeTimeSlot.BookingTime.Date);

        if (hasExistingBooking)
        {
            return BadRequest("You can only book one tee time per day.");
        }

        // Proceed with booking
        _context.TeeTimeBookings.Add(booking);
        await _context.SaveChangesAsync();
        return Ok(booking);
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
