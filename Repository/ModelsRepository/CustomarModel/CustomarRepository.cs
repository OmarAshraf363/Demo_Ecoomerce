using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;

namespace Demo.Repository.ModelsRepository.CustomarModel
{
    public class CustomarRepository : GenralRepository<Customer>, ICustomarRepository
    {
        public CustomarRepository(AppDbContext context) : base(context)
        {
        }
        public void AddFromViewModel(HomeViewModels model)
        {
            Customer newCustomar = new Customer()
            {
                FirstName = model.Customar.FirstName,
                LastName = model.Customar.LastName,
                Email = model.Customar.Email,
                Phone = model.Customar.Phone,
                City = model.Customar.City,
                State = model.Customar.State,
                Street = model.Customar.Street,
                ZipCode = model.Customar.ZipCode,

            };
            Create(newCustomar);
        }
    }
}
