using System.ComponentModel.DataAnnotations;

namespace EagleSocialAppAPI2.DTOs
{
    public class AccountCreationDTO
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
