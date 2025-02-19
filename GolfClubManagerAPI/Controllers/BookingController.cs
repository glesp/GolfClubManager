using Microsoft.AspNetCore.Mvc;
using GolfClubManagerAPI.DTOs;
using GolfClubManagerAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<ActionResult<TeeTimeBooking>> BookTeeTime([FromBody] BookingDTO bookingDTO)
    {
        try
        {
            var createdBooking = await _bookingService.CreateBookingAsync(bookingDTO);
            return CreatedAtAction(nameof(GetBookingsForDate), new { date = createdBooking.TeeTimeSlot.BookingTime.Date }, createdBooking);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("bookingsForDate/{date}")]
    public async Task<ActionResult<IEnumerable<BookingDisplayDTO>>> GetBookingsForDate(DateTime date)
    {
        var bookings = await _bookingService.GetBookingsForDate(date);
        return Ok(bookings);
    }
}