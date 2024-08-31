using Demo.Data;
using Demo.Repository.ModelsRepository.BrandModel;
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
    public class unitOfWork : IunitOfWork
    {
        private readonly AppDbContext context;
        public unitOfWork(AppDbContext context)
        {
            this.context = context;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context);
            BrandRepository = new BrandRepository(context);

            StockRepository = new StockRepository(context);
            StoreRepository = new StoreRepository(context);

            OrderItemRepository = new OrderItemRepository(context);
            OrderRepository = new OrderRepository(context);
           WishListItemsRepository = new WishListItemsRepository(context);
            WishListRepository = new WishListRepository(context);
        }

        public ICategoryRepository CategoryRepository {  get; set; }

        public IProductRepository ProductRepository {  get; set; }

        public IOrderRepository OrderRepository {  get; set; }

        public IOrderItemRepository OrderItemRepository {  get; set; }

        public IBrandRepository BrandRepository {  get; set; }

        

        public IStoreRepository StoreRepository {  get; set; }

        public IStockRepository StockRepository {  get; set; }
        public IWishListItemsRepository WishListItemsRepository { get; set; }
        public IWishListRepository WishListRepository { get; set; }

      
        public void Commit()
        {
           
        }
    }
}
