using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothesStore.Models
{
    public class Product
    {
        public long ProductID { get; set; }

        [Required(ErrorMessage = "Molimo unesite naziv proizvoda")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Molimo unesite opis")]
        public string Description { get; set; } = String.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Molimo unesite pozitivnu cenu")]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Molimo unesite kategoriju")]
        public string Category { get; set; } = String.Empty;

        // NOVO POLJE: Količina na stanju u magacinu
        [Required(ErrorMessage = "Molimo unesite količinu na stanju")]
        [Range(0, int.MaxValue, ErrorMessage = "Stanje ne može biti negativno")]
        public int Stock { get; set; } = 0;
    }
}