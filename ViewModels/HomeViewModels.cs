using Demo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class HomeViewModels
    {
        [ValidateNever]
        public List<Category> Categories { get; set; } = new List<Category>();
        [ValidateNever]

        public CustomarViewModels Customar { get; set; } = new CustomarViewModels();
        [ValidateNever]

        public List<Stock> Stocks { get; set; } = new List<Stock>();
        [ValidateNever]
        public IEnumerable<Product> Products { get; set;} = Enumerable.Empty<Product>();

        [Required(ErrorMessage = "Required")]
        
        public string Quantity { get; set; } ="1";
  
        
    public int productId { get; set; }
        //for filteration
        public int? SelectedCategory { get; set; }
        
                    public decimal? SelectedPrice { get; set; }
        
              public int? SelectedRate { get; set; }
        public bool? SelectedStock { get; set; }







    }
}
