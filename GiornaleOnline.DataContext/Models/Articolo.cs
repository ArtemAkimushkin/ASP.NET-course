using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiornaleOnline.DataContext.Models
{
    public class Articolo
    {
        public int Id { get; set; }
        [Required]
        public Utente? Autore { get; set; }
        public Categoria? Categoria { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Title { get; set; }
        [Required]
        public string? Testo { get; set; }
        [Required]
        public bool Pubblicato { get; set; }
        [Required]
        public DateTime DataCreazione { get; set; }
        [Required]
        public DateTime DataUltimaModifica { get; set; }

    }
}
