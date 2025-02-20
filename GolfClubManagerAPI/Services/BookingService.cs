using Microsoft.EntityFrameworkCore;
using GolfClubManagerAPI.Data;
using GolfClubManagerAPI.DTOs;

public class BookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeeTimeBooking>> CreateBookingAsync(BookingDTO bookingDTO)
    {
        Console.WriteLine($"📌 Debug: Checking TeeTimeSlotId = {bookingDTO.TeeTimeSlotId}");

        var teeTimeSlot = await _context.TeeTimeSlots.FindAsync(bookingDTO.TeeTimeSlotId);
        if (teeTimeSlot == null)
        {
            Console.WriteLine($"❌ ERROR: Tee Time Slot with ID {bookingDTO.TeeTimeSlotId} not found in DB.");
            throw new InvalidOperationException($"Tee Time Slot {bookingDTO.TeeTimeSlotId} not found.");
        }

        var newBookings = new List<TeeTimeBooking>();
    
        foreach (var memberId in bookingDTO.MemberIds)
        {
            if (memberId == 0) continue; // 🛑 Skip empty member slots

            newBookings.Add(new TeeTimeBooking { TeeTimeSlotId = bookingDTO.TeeTimeSlotId, MemberId = memberId });
            Console.WriteLine($"✅ Adding booking for MemberId: {memberId} at TeeTimeSlot {bookingDTO.TeeTimeSlotId}");
        }
        
        if (newBookings.Count == 0)
        {
            Console.WriteLine("❌ ERROR: No valid members selected for booking.");
            throw new InvalidOperationException("At least one valid member is required to book.");
        }
        
        // 🔥 FIX: Disable tracking before saving
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }


        _context.TeeTimeBookings.AddRange(newBookings);
        await _context.SaveChangesAsync();
        return newBookings;
    }


    public async Task<List<TeeTimeSlotDTO>> GetAvailableSlotsForDate(DateTime date)
    {
        return await _context.TeeTimeSlots
            .Where(slot => slot.BookingTime.Date == date.Date)
            .Select(slot => new TeeTimeSlotDTO
            {
                Id = slot.Id,
                BookingTime = slot.BookingTime
            })
            .ToListAsync();
    }

    public async Task<List<BookingDisplayDTO>> GetBookingsForDate(DateTime date)
    {
        return await _context.TeeTimeBookings
            .Where(b => b.TeeTimeSlot.BookingTime.Date == date.Date)
            .Include(b => b.Member)
            .Select(b => new BookingDisplayDTO
            {
                MemberName = b.Member.Name,
                BookingTime = b.TeeTimeSlot.BookingTime,
                TeeTimeSlotId = b.TeeTimeSlotId
            })
            .ToListAsync();
    }
}