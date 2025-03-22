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
        Console.WriteLine($" Debug: Checking TeeTimeSlotId = {bookingDTO.TeeTimeSlotId}");

        var teeTimeSlot = await _context.TeeTimeSlots.FindAsync(bookingDTO.TeeTimeSlotId);
        if (teeTimeSlot == null)
        {
            Console.WriteLine($" ERROR: Tee Time Slot with ID {bookingDTO.TeeTimeSlotId} not found in DB.");
            throw new InvalidOperationException($"Tee Time Slot {bookingDTO.TeeTimeSlotId} not found.");
        }

        var newBookings = new List<TeeTimeBooking>();
        var bookingDate = teeTimeSlot.BookingTime.Date; // Extract the date

    
        foreach (var memberId in bookingDTO.MemberIds)
        {
            if (memberId == 0) continue; //  Skip empty member slots
            
            var existingBookings = await _context.TeeTimeBookings
                .Include(b => b.TeeTimeSlot)
                .Where(b => b.MemberId == memberId)
                .ToListAsync();

// Log all existing bookings
            foreach (var booking in existingBookings)
            {
                Console.WriteLine($"🔍 DEBUG: Member {memberId} already booked on {booking.TeeTimeSlot.BookingTime:yyyy-MM-dd HH:mm}");
            }

            Console.WriteLine($" Requested Booking Date: {teeTimeSlot.BookingTime:yyyy-MM-dd HH:mm}");

            
            //  Check if this member already has a booking on the same day
            bool alreadyBooked = await _context.TeeTimeBookings
                .Where(b => b.MemberId == memberId)
                .Select(b => new { BookingDate = b.TeeTimeSlot.BookingTime.Date })
                .AnyAsync(b => b.BookingDate == teeTimeSlot.BookingTime.Date);


            if (alreadyBooked)
            {
                Console.WriteLine($" ERROR: Member {memberId} already booked on {bookingDate}");
                throw new InvalidOperationException($"Member {memberId} cannot book more than once per day.");
            }


            newBookings.Add(new TeeTimeBooking { TeeTimeSlotId = bookingDTO.TeeTimeSlotId, MemberId = memberId });
            Console.WriteLine($" Adding booking for MemberId: {memberId} at TeeTimeSlot {bookingDTO.TeeTimeSlotId}");
        }
        
        if (newBookings.Count == 0)
        {
            Console.WriteLine(" ERROR: No valid members selected for booking.");
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
                TeeTimeSlotId = b.TeeTimeSlotId,
                Handicap = b.Member.Handicap
            })
            .ToListAsync();
    }
    
    // This goes in your backend BookingService.cs
    public async Task<List<BookingDisplayDTO>> GetBookingsForMemberAsync(int memberId, DateTime? date = null)
    {
        // First, find the tee time slots this member has booked
        var memberBookingsQuery = _context.TeeTimeBookings
            .Where(b => b.MemberId == memberId);
        
        // Apply date filter if provided
        if (date.HasValue)
        {
            memberBookingsQuery = memberBookingsQuery
                .Include(b => b.TeeTimeSlot)
                .Where(b => b.TeeTimeSlot.BookingTime.Date == date.Value.Date);
        }
    
        // Get the tee time slot IDs this member has booked
        var teeTimeSlotIds = await memberBookingsQuery
            .Select(b => b.TeeTimeSlotId)
            .ToListAsync();
        
        Console.WriteLine($"Found {teeTimeSlotIds.Count} tee time slots for member {memberId}");
    
        // Now get ALL bookings for these tee time slots (including other members)
        var allBookingsForSlots = await _context.TeeTimeBookings
            .Include(b => b.TeeTimeSlot)
            .Include(b => b.Member)
            .Where(b => teeTimeSlotIds.Contains(b.TeeTimeSlotId))
            .ToListAsync();
        
        Console.WriteLine($"Found {allBookingsForSlots.Count} total bookings across these tee time slots");
    
        // Map to DTOs
        var result = allBookingsForSlots
            .Select(b => new BookingDisplayDTO
            {
                TeeTimeSlotId = b.TeeTimeSlotId,
                BookingTime = b.TeeTimeSlot.BookingTime,
                MemberId = b.MemberId,
                MemberName = b.Member.Name,
                Handicap = b.Member.Handicap,
                // Flag to highlight the selected member
                IsSelectedMember = b.MemberId == memberId
            })
            .OrderBy(b => b.BookingTime)
            .ThenBy(b => b.MemberName)
            .ToList();
        
        return result;
    }
    
    public async Task<List<BookingDisplayDTO>> GetAllBookingsAsync(DateTime? date = null)
    {
        var query = _context.TeeTimeBookings
            .Include(b => b.TeeTimeSlot)
            .Include(b => b.Member)
            .Select(b => new BookingDisplayDTO
            {
                MemberName = b.Member.Name,
                BookingTime = b.TeeTimeSlot.BookingTime,
                TeeTimeSlotId = b.TeeTimeSlotId,
                Handicap = b.Member.Handicap
            });

        if (date.HasValue)
        {
            query = query.Where(b => b.BookingTime.Date == date.Value.Date);
        }

        return await query.ToListAsync();
    }



}