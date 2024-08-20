using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.CategoryModel
{
    public interface ICategoryRepository:IGenralRepository<Category>
    {
    void AddFromViewModel(CategoryViewModels model);
    }
}
