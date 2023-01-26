using GiornaleOnline.DataContext;
using Microsoft.EntityFrameworkCore;

namespace GiornaleOnline.Extensions
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<GOContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore migrazione" + ex.Message);
                    }
                }
            }
            return webApp;
        }
    }
}
