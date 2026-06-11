using System.Linq;

namespace ClothesStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
        void SaveProduct(Product p);
        void UpdateProduct(Product p) => SaveProduct(p);
        void CreateProduct(Product p) => SaveProduct(p);
        void DeleteProduct(Product p);
    }
}