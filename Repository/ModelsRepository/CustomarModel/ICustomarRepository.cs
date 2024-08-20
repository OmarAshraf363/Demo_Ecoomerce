using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.CustomarModel
{
    public interface ICustomarRepository:IGenralRepository<Customer>
    {
        void AddFromViewModel(HomeViewModels model);
    }
}
