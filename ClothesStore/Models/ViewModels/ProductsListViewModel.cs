namespace ClothesStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
        public string? CurrentCategory { get; set; }
    }
}