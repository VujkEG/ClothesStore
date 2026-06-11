using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClothesStore.Models;
using System.Linq;

namespace ClothesStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;

        public CartModel(IStoreRepository repo, Cart cartService)
        {
            repository = repo;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : System.Net.WebUtility.UrlDecode(returnUrl);
        }

        public IActionResult OnPost(long productId, string returnUrl, int quantity = 1)
        {
            Product? product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                // Dodajemo tačan broj komada koji je kupac izabrao preko plus/minus kontrola
                Cart.AddItem(product, quantity);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new
                {
                    linesCount = Cart.Lines.Sum(x => x.Quantity),
                    totalValue = Cart.ComputeTotalValue().ToString("c")
                });
            }

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductID == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}