using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class Stock
    {
        public int StoreId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }

        public int? Quantity { get; set; }
        [ValidateNever]
        public virtual Product Product { get; set; } = null!;
        [ValidateNever]

        public virtual Store Store { get; set; } = null!;
    }
}
