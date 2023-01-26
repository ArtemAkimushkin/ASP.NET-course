using System.ComponentModel.DataAnnotations;

namespace GiornaleOnline.Models
{
    public class ArticoloDTO
    {
        /*[Required]
        public int AutoreId { get; set; }*/ 
        [Required]
        public int CategoriaId { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Titolo { get; set; }
        [Required]
        public string? Testo { get; set; }
        public bool Pubblicato { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataUltimaModifica { get; set; }
    }
}
