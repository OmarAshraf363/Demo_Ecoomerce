using Demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class CategoryViewModels
    {
        public int CategoryId {  get; set; }
        [Required(ErrorMessage ="Required")]
        public string? CategoryName {  get; set; }
        public List<Category> Categories { get; set; }=new List<Category>();
        public List<Category> SpacficCategories { get; set; } =new List<Category>();

    }
}
