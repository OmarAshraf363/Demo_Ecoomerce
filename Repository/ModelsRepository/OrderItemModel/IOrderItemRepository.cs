using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.OrderItemRepository
{
    public interface IOrderItemRepository:IGenralRepository<OrderItem>
    {
        IEnumerable<CartViewModel> GetOrderItemsInSpacifcCart(int? orderId);
        void createOrderItemsIfNotExisted(int productId, int orderId, int quantity);
        void spacifcDelete(int productId);
    }
}
