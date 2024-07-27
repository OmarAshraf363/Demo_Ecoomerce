using Demo.Models;

namespace Demo.ViewModels
{
    public class ProductOrderViewModels
    {
        public List<Order> Orders { get; set; }=new List<Order>();
        public Product Product { get; set; } = new Product();
    }
}
