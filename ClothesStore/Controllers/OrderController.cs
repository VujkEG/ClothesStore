using Microsoft.AspNetCore.Mvc;
using ClothesStore.Models;
using System.Linq;
using System;

namespace ClothesStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Vaša korpa je prazna!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                order.OrderDate = DateTime.Now;
                order.Status = "Aktivna";
                order.OrderPlaced = true;
                order.IsDeleted = false;
                order.Shipped = false;

                // Spajamo FirstName i LastName u jedno polje za bazu
                order.Name = $"{order.FirstName} {order.LastName}".Trim();

                // Da baza ne ostane prazna za State, kopiramo Country
                order.State = order.Country;

                repository.SaveOrder(order);
                cart.Clear();

                return RedirectToAction(nameof(Completed), new { orderId = order.OrderID });
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed(long orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}