using GiornaleOnline.DataContext.Models;
using GiornaleOnline.Models;

namespace GiornaleOnline.Extensions
{
    public static class ModelsExtensions
    {
        public static CategoriaModel ToCategoriaModel(this Categoria item)
        {
            return new CategoriaModel
            {
                Id = item.Id,
                Nome = item.Nome
            };
        }

        public static UtenteModel ToUtenteModel(this Utente item)
        {
            return new UtenteModel
            {
                Id = item.Id,
                Nome = item.Nome,
                Username = item.Username
            };
        }

        public static ArticoloModel ToArticoloModel(this Articolo item)
        {
            return new ArticoloModel
            {
                Id = item.Id,
                Autore = item.Autore!.ToUtenteModel(),
                Categoria = item.Categoria!.ToCategoriaModel(),
                Titolo = item.Title,
                Testo = item.Testo,
                DataCreazione = item.DataCreazione
            };
        }
    }
}
