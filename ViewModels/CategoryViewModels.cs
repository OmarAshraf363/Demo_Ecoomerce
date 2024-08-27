using Demo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class CategoryViewModels
    {
        public int CategoryId {  get; set; }
        [Required(ErrorMessage ="Required")]
        public string Name {  get; set; }=string.Empty;
        [ValidateNever]
        public List<Category> Categories { get; set; }=new List<Category>();
        [ValidateNever]
        public List<Category> SpacficCategories { get; set; } =new List<Category>();

    }
}
