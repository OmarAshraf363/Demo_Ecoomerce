using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;

namespace Demo.Repository.ModelsRepository.WishListItemsModel
{
    public class WishListItemsRepository : GenralRepository<WishListItems>, IWishListItemsRepository
    {
        public WishListItemsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
