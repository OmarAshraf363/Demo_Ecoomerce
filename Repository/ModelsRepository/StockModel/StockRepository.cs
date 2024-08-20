using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;

namespace Demo.Repository.ModelsRepository.StockModel
{
    public class StockRepository : GenralRepository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext context) : base(context)
        {
        }
    }
}
