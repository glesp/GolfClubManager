using System.ComponentModel.DataAnnotations;

namespace GolfClubManagerWASM.DTOs
{
    public class MemberCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Handicap is required")]
        [Range(0, 54, ErrorMessage = "Handicap must be between 0 and 54")]
        public int Handicap { get; set; }
    }
}