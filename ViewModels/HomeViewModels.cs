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

        [Required(ErrorMessage = "Required")]
        
        public string Quantity { get; set; } ="1";
  
        
    public int productId { get; set; }

       
    }
}
