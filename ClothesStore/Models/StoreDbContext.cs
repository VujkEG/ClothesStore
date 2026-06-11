using Microsoft.EntityFrameworkCore;

namespace ClothesStore.Models
{
    public class StoreDbContext : DbContext
    {
        // Konstruktor klase - povezuje bazu sa podešavanjima iz appsettings.json
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options) { }

        // Tabele u bazi podataka
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();

        // Metoda za konfiguraciju konteksta (Uklonjen problematični warning koji je rušio sajt)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        // Metoda za mapiranje modela - trenutno čista i spremna za rad sa admin panelom
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}