using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace ClothesStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin123!";
        private const string adminRole = "Admins"; // OVDE SMO DODALI PRAVILNU ULOGU SA 'S'

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppIdentityDbContext context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

                // Kreiranje Identity tabela unutar postojeće baze ako ne postoje
                var databaseCreator = context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    try
                    {
                        await databaseCreator.CreateTablesAsync();
                    }
                    catch (System.Exception)
                    {
                        // Ako tabele već postoje, SQL će baciti grešku koju ovde bezbedno ignorišemo
                    }
                }

                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Kreiramo ulogu "Admins" u bazi podataka
                if (await roleManager.FindByNameAsync(adminRole) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                }

                IdentityUser? user = await userManager.FindByNameAsync(adminUser);

                // Ako stari admin postoji, brišemo ga da bi se podaci osvežili u bazi sa novom ulogom
                if (user != null)
                {
                    await userManager.DeleteAsync(user);
                }

                // Pravimo admina ponovo
                user = new IdentityUser { UserName = adminUser, Email = "admin@example.com" };
                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    // DODELJUJEMO MU ISPRAVNU ULOGU "Admins" DA SE POKLAPA SA PROGRAM.CS
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }
    }
}