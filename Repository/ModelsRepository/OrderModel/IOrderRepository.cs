using Demo.Models;
using Demo.Repository.IRepository;

namespace Demo.Repository.ModelsRepository.OrderModel
{
    public interface IOrderRepository:IGenralRepository<Order>
    {
        Order? GetUserOrder(string? userId);
        Order CreateFirstOrderIfNotExisted(string? userId);
    }
}
