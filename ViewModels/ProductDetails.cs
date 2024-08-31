using Demo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class ProductDetails
    {
        public int? realQuantity { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Required")]

        public string Quantity { get; set; } = "1";


        public int productId { get; set; }


        public IEnumerable<Product>? Products { get; set; }
        public List<WishListItems> WishListItems { get; set; } = new List<WishListItems>();



    }
}
