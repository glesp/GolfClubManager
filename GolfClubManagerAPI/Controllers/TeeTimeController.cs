using Microsoft.AspNetCore.Mvc;
using GolfClubManagerAPI.DTOs;
using GolfClubManagerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace GolfClubManagerAPI.Controllers
{

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
            var futureDateLimit = date.AddDays(7); // Extend the search range by 7 days

            var slots = await _context.TeeTimeSlots
                .Where(slot => slot.BookingTime.Date >= date.Date && slot.BookingTime.Date <= futureDateLimit.Date) // Show slots in the next 7 days
                .Where(slot => !_context.TeeTimeBookings.Any(b => b.TeeTimeSlotId == slot.Id)) // Exclude booked slots
                .OrderBy(slot => slot.BookingTime) // Sort by time
                .Select(slot => new TeeTimeSlotDTO
                {
                    Id = slot.Id,
                    BookingTime = slot.BookingTime
                })
                .ToListAsync();

            return Ok(slots);
        }

    }
}