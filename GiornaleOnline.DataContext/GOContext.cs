using GiornaleOnline.DataContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiornaleOnline.DataContext
{
    public class GOContext : DbContext
    {
        public GOContext()
        {

        }

        public GOContext(DbContextOptions<GOContext> options)
            : base(options)
        {

        }

        public DbSet<Utente> Utenti => Set<Utente>();
        public DbSet<Categoria> Categorie => Set<Categoria>();
        public DbSet<Articolo> Articoli => Set<Articolo>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=GiornaleOnline; User Id=sa;Password=258456");
            }*/
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articolo>().Property(p => p.Pubblicato).HasDefaultValue(true);
            modelBuilder.Entity<Articolo>().Property(p => p.DataCreazione).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Articolo>().Property(p => p.DataUltimaModifica).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Utente>().HasIndex(i => i.Username).IsUnique();

            #region Data Seed (Inserimento dei dati in fase di creazione)   
            modelBuilder.Entity<Utente>().HasData(new Utente() { Id = 1, Nome = "Admin", Username = "Admin", Password = "258456" });
            modelBuilder.Entity<Categoria>().HasData(new Categoria() { Id = 1, Nome = "Cronaca" });
            #endregion
        }
    }
}
