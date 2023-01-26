using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiornaleOnline.DataContext.Models
{
    public class Utente
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Nome { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
