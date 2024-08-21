using Microsoft.AspNetCore.Identity;

namespace Demo.Models
{
    public class AppUser:IdentityUser
    {
        public string? Address {  get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
