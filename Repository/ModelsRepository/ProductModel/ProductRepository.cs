using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Demo.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repository.ModelsRepository.ProductModel
{
    public class ProductRepository : GenralRepository<Product>, IProductRepository
    {
        AppDbContext context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            this.context= context;
        }

        public int createFromViewModel(ProductsViewModels model)
        {
            Product product = new Product()
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Image = model.Image,
                ListPrice = model.ListPrice,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId,
                ModelYear = model.ModelYear,
                Rate = model.Rate,


            };
            Create(product);
            return product.ProductId;
        }

        public ProductsViewModels editFromViewModel(ProductsViewModels model)
        {
            var product = context.Products.Where(e => e.ProductId == model.ProductId).SingleOrDefault();


            product.ProductName = model.ProductName;
            product.ProductDescription = model.ProductDescription;
            product.BrandId = model.BrandId;
            product.CategoryId = model.CategoryId;
            product.ModelYear = model.ModelYear;
            product.Rate = model.Rate;
            product.Image = model.Image;
            product.ListPrice = model.ListPrice;
           Edit(product);
            return model;

        }

        public AddProductFromCategoryViewModel getAllProductsWithspacifsCategory(int? categoryId)
        {
            var AllMob = context.Products.Include(e => e.Category).Include(e => e.Brand).Where(e => e.Category.CategoryId == categoryId).ToList();
            var category = context.Categories.Find(categoryId);
            var brans = context.Brands.ToList();
            AddProductFromCategoryViewModel model = new AddProductFromCategoryViewModel()
            {
                Products = AllMob,
                CategoryId = categoryId,
                CategoryName = category.CategoryName,
                Brands = brans
            };
            return model;
        }

        public ProductsViewModels putAllInfoInProductViewModel(ProductsViewModels model)
        {
           var products=Get(expression:null,e=>e.Brand,e=>e.Category).ToList();
            var categories = context.Categories.ToList();
            var brands=context.Brands.ToList();

            model.Categories = categories;
            model.Brands = brands;
            model.Products = products;
            
            return model;
            
        }
    }
}
