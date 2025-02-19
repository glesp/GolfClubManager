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

    public async Task<TeeTimeBooking> CreateBookingAsync(BookingDTO bookingDTO)
    {
        var hasBookedToday = await _context.TeeTimeBookings
            .AnyAsync(b => b.MemberId == bookingDTO.MemberId &&
                           b.TeeTimeSlot.BookingTime.Date == _context.TeeTimeSlots
                               .Where(t => t.Id == bookingDTO.TeeTimeSlotId)
                               .Select(t => t.BookingTime.Date)
                               .FirstOrDefault());

        if (hasBookedToday)
            throw new InvalidOperationException("You can only book one slot per day.");

        var bookingsCount = await _context.TeeTimeBookings
            .CountAsync(b => b.TeeTimeSlotId == bookingDTO.TeeTimeSlotId);

        if (bookingsCount >= 4)
            throw new InvalidOperationException("This slot is already full. Max 4 members per slot.");

        var booking = new TeeTimeBooking
        {
            MemberId = bookingDTO.MemberId,
            TeeTimeSlotId = bookingDTO.TeeTimeSlotId
        };

        _context.TeeTimeBookings.Add(booking);
        await _context.SaveChangesAsync();

        return booking;
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