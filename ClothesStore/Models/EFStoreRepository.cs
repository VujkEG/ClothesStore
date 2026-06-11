using System.Linq;

namespace ClothesStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext context;

        public EFStoreRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;

        public void SaveProduct(Product p)
        {
            if (p.ProductID == 0)
            {
                context.Products.Add(p);
            }
            else
            {
                Product? dbEntry = context.Products
                    .FirstOrDefault(x => x.ProductID == p.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = p.Name;
                    dbEntry.Description = p.Description;
                    dbEntry.Price = p.Price;
                    dbEntry.Category = p.Category;
                    dbEntry.Stock = p.Stock;
                }
            }
            context.SaveChanges();
        }

        public void DeleteProduct(Product p)
        {
            context.Products.Remove(p);
            context.SaveChanges();
        }
    }
}