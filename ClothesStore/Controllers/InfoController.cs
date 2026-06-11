using Microsoft.AspNetCore.Mvc;

namespace ClothesStore.Controllers
{
	public class InfoController : Controller
	{
		public IActionResult KakoKupiti()
		{
			return View();
		}

		public IActionResult PovracajRobe()
		{
			return View();
		}

		public IActionResult UsloviKoriscenja()
		{
			return View();
		}
	}
}