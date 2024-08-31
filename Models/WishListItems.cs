using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class WishListItems
    {
        [Key]
        public int Id { get; set; }
        public int WishListId { get; set; }
        [ForeignKey("WishListId")]
        public WishList? WishList { get; set; }
        public int ProductId {  get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
