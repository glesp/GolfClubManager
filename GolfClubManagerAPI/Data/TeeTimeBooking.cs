using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GolfClubManagerAPI.Data;

namespace GolfClubManagerAPI.Data
{
    public class TeeTimeBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeeTimeSlotId { get; set; } // FK for tee time slot

        [Required]
        public int MemberId { get; set; } // FK for member

        // Navigation properties
        [ForeignKey("TeeTimeSlotId")]
        public TeeTimeSlot TeeTimeSlot { get; set; } = null!;

        [ForeignKey("MemberId")]
        public Member Member { get; set; } = null!;
    }
}