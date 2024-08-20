using Demo.Models;

namespace Demo.ViewModels
{
    public class BrandViewModels
    {
        public List<Brand> Brands { get; set; }=new List<Brand>();
        public Brand? Brand { get; set; }
        public int BrandId { get; set; }

        public string BrandName { get; set; } = null!;
    }
}
