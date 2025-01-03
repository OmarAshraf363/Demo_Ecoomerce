﻿using Demo.Repository.ModelsRepository.BrandModel;
using Demo.Repository.ModelsRepository.CategoryModel;

using Demo.Repository.ModelsRepository.OrderItemRepository;
using Demo.Repository.ModelsRepository.OrderModel;
using Demo.Repository.ModelsRepository.ProductModel;

using Demo.Repository.ModelsRepository.StockModel;
using Demo.Repository.ModelsRepository.StoreModel;
using Demo.Repository.ModelsRepository.WishListItemsModel;
using Demo.Repository.ModelsRepository.WishListModel;

namespace Demo.Repository.IRepository
{
    public interface IunitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }
        public IBrandRepository BrandRepository { get; }
       
        public IStoreRepository StoreRepository { get; }
        public IStockRepository StockRepository { get; }
        public IWishListRepository WishListRepository { get; }
        public IWishListItemsRepository WishListItemsRepository { get; }
      
        public void Commit();
    }
}
