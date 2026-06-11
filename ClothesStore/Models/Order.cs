using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace ClothesStore.Models
{
    public class Order
    {
        [BindNever]
        public long OrderID { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        [BindNever]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Molimo unesite poštanski broj.")]
        public string? Zip { get; set; }

        // Sklonjen Required jer se ne koristi na formi direktno
        public string? State { get; set; }

        [BindNever]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [BindNever]
        public string Status { get; set; } = "Aktivna";

        [BindNever]
        public bool OrderPlaced { get; set; }

        [Required(ErrorMessage = "Molimo unesite ime.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Molimo unesite prezime.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Molimo unesite adresu.")]
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        [Required(ErrorMessage = "Molimo unesite grad.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Molimo unesite državu.")]
        public string? Country { get; set; }

        // NOVO POLJE: Za automatsko slanje imejlova
        [Required(ErrorMessage = "Molimo unesite email adresu.")]
        [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu.")]
        public string? Email { get; set; }

        // NOVO POLJE: Za interne beleške i komentare administratora
        public string? Notes { get; set; }

        [BindNever]
        public bool Shipped { get; set; }

        [BindNever]
        public bool IsDeleted { get; set; } = false;

        public bool GiftWrap { get; set; }
    }
}