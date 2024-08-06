using Demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class CustomarViewModels
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
    }
}
