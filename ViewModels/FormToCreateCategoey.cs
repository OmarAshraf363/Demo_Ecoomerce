using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class FormToCreateCategoey
    {
        [Required]
        public string CategoryName { get; set; } = null!;
    }
}
