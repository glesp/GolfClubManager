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
        var teeTimeSlot = await _context.TeeTimeSlots.FindAsync(bookingDTO.TeeTimeSlotId);
        if (teeTimeSlot == null) throw new InvalidOperationException("Tee Time Slot not found.");

        var newBookings = new List<TeeTimeBooking>();
    
        foreach (var memberId in bookingDTO.MemberIds)
        {
            newBookings.Add(new TeeTimeBooking { TeeTimeSlotId = bookingDTO.TeeTimeSlotId, MemberId = memberId });
        }
    
        foreach (var player in bookingDTO.NewPlayers ?? new List<PlayerDTO>())
        {
            var member = new Member { Name = $"{player.FirstName} {player.LastName}" };
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            newBookings.Add(new TeeTimeBooking { TeeTimeSlotId = bookingDTO.TeeTimeSlotId, MemberId = member.Id });
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