using Microsoft.AspNetCore.Mvc;
using ClothesStore.Models;
using System.Linq;

namespace ClothesStore.Controllers
{
    public class CartController : Controller
    {
        private IStoreRepository repository;
        private Cart cart;

        public CartController(IStoreRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        // POPRAVLJENO: Sada prihvata tačnu količinu (quantity) i vraća JSON podatke za instant AJAX osvežavanje bez refresha!
        [HttpPost]
        public IActionResult AddToCart(long productId, int quantity, string returnUrl)
        {
            // Ako iz nekog razloga količina nije prosleđena, stavljamo podrazumevano 1
            if (quantity <= 0) quantity = 1;

            Product? product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, quantity);
            }

            // KLJUČNI DEO: Ako je zahtev poslat preko AJAX-a (JavaScript), šaljemo samo nove brojke nazad
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    linesCount = cart.Lines.Sum(x => x.Quantity),
                    totalValue = cart.ComputeTotalValue().ToString("#,##0") + " RSD"
                });
            }

            // Ako se desi običan klik bez JavaScripta, radi standardni fallback redirekt
            return Redirect(returnUrl ?? "/");
        }

        public RedirectResult RedirectToPage(long productId, string returnUrl)
        {
            Product? product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return Redirect(returnUrl ?? "/");
        }

        public RedirectResult RemoveFromPage(long productId, string returnUrl)
        {
            Product? product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return Redirect(returnUrl ?? "/");
        }
    }
}