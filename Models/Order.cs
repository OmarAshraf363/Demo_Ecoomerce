using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int? StoreId { get; set; }

        public byte OrderStatus { get; set; }

        public DateOnly OrderDate { get; set; }

        public DateOnly RequiredDate { get; set; }

        public DateOnly? ShippedDate { get; set; }

       

        public string? AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser? AppUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; } = null!;
    }
}
