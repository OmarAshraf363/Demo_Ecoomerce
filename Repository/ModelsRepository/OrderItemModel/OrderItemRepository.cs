using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repository.ModelsRepository.OrderItemRepository
{
    public class OrderItemRepository : GenralRepository<OrderItem>, IOrderItemRepository
    {
        AppDbContext context;
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

       public IEnumerable<CartViewModel> GetOrderItemsInSpacifcCart(int? orderId)
        {
            var orderItems = Get(e=>e.OrderId==orderId,e=>e.Product)
             .Select(e => new CartViewModel
             {
                 ProductId = e.ProductId,
                 ProductName = e.Product.ProductName,
                 Quantity = e.Quantity,
                 ListPrice = e.ListPrice,
                 TotalPrice = e.Quantity * e.ListPrice
                 
             }).ToList();

            return orderItems;
        }
       public void createOrderItemsIfNotExisted(int productId, int orderId,int quantity)
        {
            var orderitems = GetOne(e => e.ProductId == productId && e.OrderId == orderId);
              
            if (orderitems == null)
            {
                orderitems = new OrderItem()
                {
                    ItemId = productId,
                    OrderId = orderId,
                    ProductId = productId,
                    Quantity = quantity,
                    ListPrice = context.Products.First(p => p.ProductId == productId).ListPrice,
                    Discount = 0
                };
               Create(orderitems);
            }else
            {
                orderitems.Quantity += quantity;
            }
        }

        public void spacifcDelete(int productId)
        {
           var orderitem = context.OrderItems.FirstOrDefault(e => e.ProductId == productId);
            if (orderitem != null)
            {
                context.OrderItems.Remove(orderitem);
                context.SaveChanges();
            }
        }
    }
}
