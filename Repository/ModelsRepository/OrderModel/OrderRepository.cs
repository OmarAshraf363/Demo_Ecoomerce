﻿using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;

namespace Demo.Repository.ModelsRepository.OrderModel
{
    public class OrderRepository : GenralRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public Order? GetUserOrder(int? userId)
        {
           return GetOne(e=>e.CustomerId==userId && e.OrderStatus==0);
        }
        public Order CreateFirstOrderIfNotExisted(int? userId)
        {
            var order=GetUserOrder(userId);
            if (order == null)
            {
                order=new Order()
                {
                    OrderStatus = 0,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    CustomerId = userId,
                };
                Create(order);
            }
            return order;
            
        }
    }
}