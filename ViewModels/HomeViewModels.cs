using Demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class HomeViewModels
    {
        public List<Category> Categories { get; set; }=new List<Category>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public CustomarViewModels Customar { get; set; } = new CustomarViewModels();
    

       
    }
}
