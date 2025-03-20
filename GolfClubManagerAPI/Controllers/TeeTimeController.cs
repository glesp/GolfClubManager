using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolfClubManagerAPI.Data;
using GolfClubManagerAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClubManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeeTimeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeeTimeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeeTimeSlotDTO>>> GetTeeTimeSlots(DateTime date)
        {
            // Ensure we have tee time slots for the requested date range
            await EnsureTeeTimeSlotsExistAsync(date, 7);

            var futureDateLimit = date.AddDays(7); // Extend the search range by 7 days
            
            // Query existing slots in the next 7 days
            return await _context.TeeTimeSlots
                .Where(slot => slot.BookingTime.Date >= date.Date && slot.BookingTime.Date <= futureDateLimit.Date) // Show slots in the next 7 days
                .Where(slot => !_context.TeeTimeBookings.Any(b => b.TeeTimeSlotId == slot.Id)) // Exclude booked slots
                .OrderBy(slot => slot.BookingTime) // Sort by time
                .Select(slot => new TeeTimeSlotDTO
                {
                    Id = slot.Id,
                    BookingTime = slot.BookingTime
                })
                .ToListAsync();
        }

        // New method to ensure tee time slots exist
        private async Task EnsureTeeTimeSlotsExistAsync(DateTime startDate, int daysToGenerate)
        {
            // Generate slots from the start date up to the number of days specified
            for (int day = 0; day < daysToGenerate; day++)
            {
                var currentDate = startDate.AddDays(day).Date;
                
                // Check if we already have slots for this date
                var existingSlotsForDate = await _context.TeeTimeSlots
                    .AnyAsync(s => s.BookingTime.Date == currentDate);
                
                if (!existingSlotsForDate)
                {
                    // Create slots for this date
                    var newSlots = GenerateTeeTimeSlotsForDate(currentDate);
                    await _context.TeeTimeSlots.AddRangeAsync(newSlots);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private List<TeeTimeSlot> GenerateTeeTimeSlotsForDate(DateTime date)
        {
            var slots = new List<TeeTimeSlot>();
            
            // Generate tee times from 7:00 AM to 5:00 PM with 15-minute intervals
            DateTime startTime = date.Date.AddHours(7); // 7:00 AM
            DateTime endTime = date.Date.AddHours(17); // 5:00 PM
            
            while (startTime < endTime)
            {
                slots.Add(new TeeTimeSlot
                {
                    BookingTime = startTime
                });
                
                startTime = startTime.AddMinutes(15); // 15-minute intervals
            }
            
            return slots;
        }
    }
}