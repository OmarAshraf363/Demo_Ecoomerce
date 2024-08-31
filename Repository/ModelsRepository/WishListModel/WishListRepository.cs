using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;

namespace Demo.Repository.ModelsRepository.WishListModel
{
    public class WishListRepository : GenralRepository<WishList>, IWishListRepository
    {
        public WishListRepository(AppDbContext context) : base(context)
        {
        }
    }
}
