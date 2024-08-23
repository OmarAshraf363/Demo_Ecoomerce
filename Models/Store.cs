using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        [Required]
        public string StoreName { get; set; } = null!;
        [Required]
        public string? Phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Street { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? ZipCode { get; set; }
        [ValidateNever]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        [ValidateNever]

        public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
        [ValidateNever]

        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}