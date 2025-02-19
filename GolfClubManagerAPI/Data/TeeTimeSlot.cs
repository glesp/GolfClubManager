using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GolfClubManagerAPI.Data;

namespace GolfClubManagerAPI.Data
{
    public class TeeTimeSlot
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BookingTime { get; set; } // Time for the slot

        // Navigation property to related bookings
        public List<TeeTimeBooking> Bookings { get; set; } = new();
    }
}