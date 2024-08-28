using Demo.Models;

namespace Demo.ViewModels
{
    public class ProductDetails
    {
        public Product? Product { get; set; }
        public int? Quantity {  get; set; }

        public IEnumerable<Product>? Products { get; set; }
    }
}
