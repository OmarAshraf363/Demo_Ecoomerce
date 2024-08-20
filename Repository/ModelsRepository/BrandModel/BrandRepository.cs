using Azure.Core;
using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Repository.ModelsRepository.BrandModel
{
    public class BrandRepository : GenralRepository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext context) : base(context)
        {
        }

        public void AddFromViewModel(BrandViewModels model)
        {
            Brand brand = new Brand()
            {
                BrandName = model.BrandName,
            };
            Create(brand);
           
        }
        
    }
}
