using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ClothesStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private StoreDbContext context;

        public EFOrderRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product));

            if (order.OrderID == 0)
            {
                foreach (var line in order.Lines)
                {
                    var dbProduct = context.Products.FirstOrDefault(p => p.ProductID == line.Product.ProductID);
                    if (dbProduct != null)
                    {
                        if (dbProduct.Stock >= line.Quantity)
                        {
                            dbProduct.Stock -= line.Quantity;
                        }
                        else
                        {
                            dbProduct.Stock = 0;
                        }
                    }
                }
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }

        // Koristi se za regularno ažuriranje statusa i beleški
        public void UpdateOrder(Order order)
        {
            var dbOrder = context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
            if (dbOrder != null)
            {
                dbOrder.Status = order.Status;
                dbOrder.Shipped = order.Shipped;
                dbOrder.IsDeleted = order.IsDeleted;
                dbOrder.Notes = order.Notes;       // SPAS: Sada se beleške trajno snimaju!
                dbOrder.Email = order.Email;       // Spas: Čuvamo i email

                context.SaveChanges();
            }
        }

        public void DeleteOrder(Order order)
        {
            var dbOrder = context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
            if (dbOrder != null)
            {
                dbOrder.Shipped = true;
                dbOrder.Status = "Obrisana";
                dbOrder.IsDeleted = false;

                context.SaveChanges();
            }
        }
    }
}