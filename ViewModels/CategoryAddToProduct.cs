using Demo.Models;

namespace Demo.ViewModels
{
    public class CategoryAddToProduct
    {
        public List<Category> Categories { get; set; }=new List<Category>();
        public int? CategoryId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public string catname {  get; set; }



        //to add Product
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; }
        public int Rate { get; set; }
        public string Image { get; set; } = null!;
        public short ModelYear { get; set; }

        public decimal ListPrice { get; set; }
        public int BrandId { get; set; }
        public List<Brand> Brands { get; set; } = new List<Brand>();
    }
}
