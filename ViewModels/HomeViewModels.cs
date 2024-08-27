using Demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class HomeViewModels
    {
        public List<Category> Categories { get; set; }=new List<Category>();
       
        public CustomarViewModels Customar { get; set; } = new CustomarViewModels();
        public List<Stock> Stocks { get; set; }=new List<Stock>();


        [Required]
        [MinLength(1)]
        
        public int? Quantity {  get; set; }
    

       
    }
}
