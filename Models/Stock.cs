using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class Stock
    {
        public int StoreId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;

        public virtual Store Store { get; set; } = null!;
    }
}
