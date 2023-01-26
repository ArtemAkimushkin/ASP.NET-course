using System.ComponentModel.DataAnnotations;

namespace GiornaleOnline.Models
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
