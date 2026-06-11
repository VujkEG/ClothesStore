namespace ClothesStore.Models.ViewModels
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; } = new();
        public string ReturnUrl { get; set; } = "/";
    }
}