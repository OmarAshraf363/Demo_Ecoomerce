using Demo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class BrandViewModels
    {
        [ValidateNever]
        public List<Brand>? Brands { get; set; }
        

       
        

        public int BrandId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
