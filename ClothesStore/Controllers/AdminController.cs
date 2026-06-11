using Microsoft.AspNetCore.Mvc;
using ClothesStore.Models;
using System.Linq;

namespace ClothesStore.Controllers
{
	public class AdminController : Controller
	{
		private IStoreRepository repository;

		public AdminController(IStoreRepository repo)
		{
			repository = repo;
		}

		public ViewResult Index() => View(repository.Products);

		public ViewResult Edit(long productId) =>
			View(repository.Products
				.FirstOrDefault(p => p.ProductID == productId));

		[HttpPost]
		public IActionResult Edit(Product product)
		{
			if (ModelState.IsValid)
			{
				repository.SaveProduct(product);
				TempData["message"] = $"{product.Name} je sačuvan.";
				return RedirectToAction("Index");
			}
			else
			{
				return View(product);
			}
		}

		public ViewResult Create() => View("Edit", new Product());

		[HttpPost]
		public IActionResult Delete(long productId)
		{
			Product? deletedProduct = repository.Products
				.FirstOrDefault(p => p.ProductID == productId);
			if (deletedProduct != null)
			{
				repository.DeleteProduct(deletedProduct);
				TempData["message"] = $"{deletedProduct.Name} je obrisan.";
			}
			return RedirectToAction("Index");
		}
	}
}
