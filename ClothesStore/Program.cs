using Microsoft.EntityFrameworkCore;
using ClothesStore.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. DODAVANJE KONTROLERA I RAZOR PAGES/BLAZOR SERVISI
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/Admin", "Admins");
});
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();

// Autorizaciona politika "Admins" za bezbedan pristup Admin Panelu
builder.Services.AddAuthorization(options => {
    options.AddPolicy("Admins", policy => policy.RequireRole("Admins"));
});

// 2. POVEZIVANJE SA BAZOM PODATAKA (SQL Server)
builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ClothesStoreConnection"]);
});

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]));

// Postavljanje pravila za lozinke
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;             // MAKAR JEDAN BROJ
    options.Password.RequiredLength = 8;              // MINIMUM 8 KARAKTERA
    options.Password.RequireUppercase = true;         // MAKAR JEDNO VELIKO SLOVO
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppIdentityDbContext>();

// Konfigurisanje kolačića za Login i Admin Panel
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/"; // Ako običan korisnik pokuša da upadne na /admin, vraća ga na Home
});

// 3. REGISTRACIJA REPOZITORIJUMA
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

// 4. REGISTRACIJA HTTPCONTEXTACCESSOR-A
builder.Services.AddHttpContextAccessor();

// 5. REGISTRACIJA KORPE
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

// 6. AKTIVACIJA MEMORIJSKOG KEŠA I SESIJE
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.Cookie.Name = ".ClothesStore.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 7. MIDDLEWARE KORACI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// 8. MAPIRANJE RUTA
app.MapControllerRoute(
    name: "catpage",
    pattern: "{category}/Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "page",
    pattern: "Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "category",
    pattern: "{category}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

// 9. BEZBEDNO PUNJENJE BAZE (Sprečava rušenje aplikacije na samom startu)
try
{
    SeedData.EnsurePopulated(app);
    await IdentitySeedData.EnsurePopulated(app);
}
catch (Exception ex)
{
    Console.WriteLine("-------------------------------------------------------------------");
    Console.WriteLine($"[SISTEMSKA NOTIFIKACIJA] Sajt je pokrenut, ali baza javlja grešku: {ex.Message}");
    Console.WriteLine("-------------------------------------------------------------------");
}

app.Run();