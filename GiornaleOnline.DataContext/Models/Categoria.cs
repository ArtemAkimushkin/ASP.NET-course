using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiornaleOnline.DataContext.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Nome { get; set; }
        public virtual ICollection<Articolo> Articoli { get; set; } = new List<Articolo>();
    }
}
