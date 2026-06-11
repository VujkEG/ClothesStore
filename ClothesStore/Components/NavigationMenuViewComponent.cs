using Microsoft.AspNetCore.Mvc;
using ClothesStore.Models;
using System.Linq;

namespace ClothesStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository repository;

        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            // Selektuje trenutnu kategoriju iz URL-a ako postoji
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            // Izvlači jedinstvene kategorije i sortira ih azbučno
            var categories = repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(categories);
        }
    }
}