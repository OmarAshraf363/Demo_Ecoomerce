using Demo.Models;

namespace Demo.ViewModels
{
    public class AddProductFromCategoryViewModel
    {
        public List<Category> Categories { get; set; }=new List<Category>();
        public int? CategoryId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public string CategoryName {  get; set; }=string.Empty;



        //to add Productv//ProductModel
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = string.Empty;
        public int Rate { get; set; }
        public string Image { get; set; } = null!;
        public short ModelYear { get; set; }

        public decimal ListPrice { get; set; }
        public int BrandId { get; set; }
        public List<Brand> Brands { get; set; } = new List<Brand>();
    }
}
