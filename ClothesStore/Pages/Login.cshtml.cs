using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClothesStore.Pages
{
    public class LoginModel : PageModel
    {
        private SignInManager<IdentityUser> signInManager;

        public LoginModel(SignInManager<IdentityUser> signinMgr)
        {
            signInManager = signinMgr;
        }

        [BindProperty]
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        public string? Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [BindProperty]
        public string? ReturnUrl { get; set; }

        public void OnGet(string? returnUrl)
        {
            ReturnUrl = returnUrl ?? "/Admin";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Brišemo sistemsku grešku za returnUrl ako je .NET sam generisao
            ModelState.Remove("returnUrl");
            ModelState.Remove("ReturnUrl");

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(Name!, Password!, false, false);
                if (result.Succeeded)
                {
                    return Redirect(ReturnUrl ?? "/Admin");
                }
            }
            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka");
            return Page();
        }
    }
}