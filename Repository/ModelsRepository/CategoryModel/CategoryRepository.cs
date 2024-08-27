using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.CategoryModel
{
    public class CategoryRepository : GenralRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public void AddFromViewModel(CategoryViewModels model)
        {
           Category category=new Category()
           {
               CategoryName = model.Name,
               
           };
            Create(category);

        }
    }
}
