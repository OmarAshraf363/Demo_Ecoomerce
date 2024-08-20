using Demo.Models;
using Demo.Repository.IRepository;

namespace Demo.Repository.ModelsRepository.OrderModel
{
    public interface IOrderRepository:IGenralRepository<Order>
    {
        Order? GetUserOrder(int? userId);
        Order CreateFirstOrderIfNotExisted(int? userId);
    }
}
