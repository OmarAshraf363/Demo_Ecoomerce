using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }


        public string? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser? AppUser { get; set; }
        public virtual ICollection<WishListItems> WishListItems { get; set; }=new List<WishListItems>();

    }
}
