using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace ClothesStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return Redirect("/");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Korisničko ime i lozinka su obavezni.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(username.Trim(), password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Redirect("/");
            }

            ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka.");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return Redirect("/");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string fullName, string password, string confirmPassword)
        {
            ViewBag.Username = username;
            ViewBag.FullName = fullName;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError("", "Sva polja su obavezna i moraju biti popunjena.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Lozinke se ne podudaraju.");
                return View();
            }

            var user = new IdentityUser { UserName = username.Trim() };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/");
            }

            foreach (var error in result.Errors)
            {
                string description = error.Description;
                if (error.Code == "DuplicateUserName") description = "Korisničko ime je već zauzeto.";
                if (error.Code == "PasswordTooShort") description = "Lozinka mora imati najmanje 8 karaktera.";
                if (error.Code == "PasswordRequiresDigit") description = "Lozinka mora da sadrži najmanje jedan broj (0-9).";
                if (error.Code == "PasswordRequiresUpper") description = "Lozinka mora da sadrži najmanje jedno veliko slovo (A-Z).";

                ModelState.AddModelError("", description);
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}