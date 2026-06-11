using Microsoft.AspNetCore.Mvc;
using ClothesStore.Models;
using ClothesStore.Models.ViewModels;
using System.Linq;

namespace ClothesStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository repository;
        public int PageSize = 4; // Broj proizvoda po stranici

        public HomeController(IStoreRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string? category, int productPage = 1)
        {
            // POPRAVKA: Ako je prosleđen prazan string za kategoriju, pretvaramo ga u null da povuče sve proizvode
            if (string.IsNullOrEmpty(category))
            {
                category = null;
            }

            var products = repository.Products
                .Where(p => category == null || p.Category == category);

            var model = new ProductsListViewModel
            {
                Products = products
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = products.Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }
    }
}