namespace GiornaleOnline.Models
{
    public class ArticoloModel
    {
        public int Id { get; set; }
        public UtenteModel? Autore { get; set; }
        public CategoriaModel? Categoria { get; set; }
        public string? Titolo { get; set; }
        public string? Testo { get; set; }
        public DateTime DataCreazione { get; set; }
    }
}
