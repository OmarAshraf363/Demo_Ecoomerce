using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class AppUser:IdentityUser
    {
        public string? Address {  get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        //public int? StoreId {  get; set; }
        //[ForeignKey(nameof(StoreId))]
        //public Store? Store { get; set; }
    }
}
