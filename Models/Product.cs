using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; }
        public int Rate {  get; set; }
        public string Image {  get; set; } = null!;

        [ForeignKey(nameof(BrandId))]
        public int BrandId { get; set; }

        public decimal? Discount {  get; set; }

        public int CategoryId { get; set; }

        public short ModelYear { get; set; }

        public decimal ListPrice { get; set; }

        public virtual Brand Brand { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public virtual ICollection<WishListItems> WishListItems { get; set; } = new List<WishListItems>();


    }
}
