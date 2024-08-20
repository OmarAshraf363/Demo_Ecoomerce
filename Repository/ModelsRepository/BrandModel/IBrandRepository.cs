using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.BrandModel
{
    public interface IBrandRepository:IGenralRepository<Brand>
    {
        void AddFromViewModel(BrandViewModels models);
    }
}
