using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClothesStore.Models;

namespace ClothesStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                StoreDbContext context = scope.ServiceProvider
                    .GetRequiredService<StoreDbContext>();

                // 1. Ako ima beklog migracija, izvrši ih automatski
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                // 2. AUTOMATSKI REFRESH: Ako je baza prazna ILI ako detektuje stare nazive proizvoda, radi se restart
                if (!context.Products.Any() || context.Products.Any(p => p.Name == "Stana rastegljiva majica"))
                {
                    // Čistimo stare proizvode koji nemaju odgovarajuće slike
                    if (context.Products.Any())
                    {
                        context.Products.RemoveRange(context.Products);
                        context.SaveChanges();
                    }

                    context.Products.AddRange(
                        new Product { Name = "Oversized bela majica", Description = "Moderni oversized kroj, 100% vrhunski češljani pamuk.", Category = "Majice", Price = 1800, Stock = 12 },
                        new Product { Name = "Crni duks bez kapuljače", Description = "Klasičan crewneck duks, mekani i topli keper unutra.", Category = "Duksevi", Price = 3900, Stock = 8 },
                        new Product { Name = "Narandzasti duks bez kapuljače", Description = "Upečatljiv duks u modernoj boji, savršen za kežual kombinacije.", Category = "Duksevi", Price = 3500, Stock = 4 },
                        new Product { Name = "Kožna jakna", Description = "Moderna bajkerska jakna od 100% prirodne jagnjeće kože.", Category = "Jakne", Price = 12500, Stock = 2 },
                        new Product { Name = "Zimska parka jakna", Description = "Vodootporna jakna sa bogatim krznom na kapuljači i termo postavom.", Category = "Jakne", Price = 9800, Stock = 6 },
                        new Product { Name = "Teksas jakna klasična", Description = "Kvalitetan i dugotrajan neelastični teksas, vintage plava nijansa.", Category = "Jakne", Price = 4500, Stock = 15 },
                        new Product { Name = "Slim-fit plave farmerke", Description = "Kvalitetan teksas sa blagim fabričkim oštećenjima i elastinom.", Category = "Pantalone", Price = 4200, Stock = 25 },
                        new Product { Name = "Crne kargo pantalone", Description = "Pantalone sa džepovima sa strane, izdržljiv i jak ripstop materijal.", Category = "Pantalone", Price = 4800, Stock = 3 },
                        new Product { Name = "Sportske patike", Description = "Lagane i izuzetno udobne patike za trčanje sa vazdušnim đonom.", Category = "Obuća", Price = 7900, Stock = 9 },
                        new Product { Name = "Kožne jesenje cipele", Description = "Elegantne i kvalitetne muške cipele od prave prevrnute kože.", Category = "Obuća", Price = 8900, Stock = 5 },
                        new Product { Name = "Bordo torbica", Description = "Elegantna ženska torbica od fine eko-kože sa zlatnim detaljima.", Category = "Aksesoari", Price = 3200, Stock = 10 }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}