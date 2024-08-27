using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.ProductModel
{
    public interface IProductRepository:IGenralRepository<Product>
    {
        ProductsViewModels putAllInfoInProductViewModel(ProductsViewModels model);

        AddProductFromCategoryViewModel getAllProductsWithspacifsCategoryOrBrand(int? id, bool brand);
        int createFromViewModel(ProductsViewModels model);
        ProductsViewModels editFromViewModel(ProductsViewModels model);

        ProductsViewModels PrepareProductViewModel(Product product = null);
    }
}
