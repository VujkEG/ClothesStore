using System.Linq;

namespace ClothesStore.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
        void UpdateOrder(Order order); // DODATO: Za sigurno ažuriranje statusa i premeštanje po sekcijama
        void DeleteOrder(Order order);
    }
}