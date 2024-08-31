using Demo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.ViewModels
{
    public class ProductsViewModels
    {
        public int ProductId {  get; set; }
        public Product? Product { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set;} = new List<Category>();
        [Required(ErrorMessage ="Name Is Requerd")]
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; }
        public int Rate { get; set; }
        public string Image { get; set; } = null!;
        public short ModelYear { get; set; }
     
        public decimal ListPrice { get; set; }
      
       public int Quantity {  get; set; }
        public int StoreId {  get; set; }
        public List<Store> Stores { get; set; }=new List<Store>() ;
        public int BrandId { get; set; }

        public int CategoryId { get; set; }
        public List<Brand> Brands { get; set; }= new List<Brand>();



    }
}
