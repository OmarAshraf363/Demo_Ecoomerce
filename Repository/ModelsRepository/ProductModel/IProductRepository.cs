using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.ProductModel
{
    public interface IProductRepository:IGenralRepository<Product>
    {
        ProductsViewModels putAllInfoInProductViewModel(ProductsViewModels model);
        
        AddProductFromCategoryViewModel getAllProductsWithspacifsCategory(int? categoryId);
        int createFromViewModel(ProductsViewModels model);
        ProductsViewModels editFromViewModel(ProductsViewModels model);
    }
}
