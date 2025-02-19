using GolfClubManagerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using GolfClubManagerAPI.DTOs;


namespace GolfClubManagerAPI.Controllers
{

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
        public async Task<ActionResult> BookTeeTime([FromBody] BookingDTO bookingDTO)
        {
            if (bookingDTO.MemberIds.Count == 0 && (bookingDTO.NewPlayers == null || bookingDTO.NewPlayers.Count == 0))
                return BadRequest("At least one player is required.");

            var createdBookings = await _bookingService.CreateBookingAsync(bookingDTO);
            return Ok(createdBookings);
        }


        [HttpGet("bookingsForDate/{date}")]
        public async Task<ActionResult<IEnumerable<BookingDisplayDTO>>> GetBookingsForDate(DateTime date)
        {
            var bookings = await _bookingService.GetBookingsForDate(date);
            return Ok(bookings);
        }
    }
}