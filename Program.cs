using Demo.Data;
using Demo.Repository.IRepository;
using Demo.Repository.ModelsRepository.BrandModel;
using Demo.Repository.ModelsRepository.CategoryModel;
using Demo.Repository.ModelsRepository.CustomarModel;
using Demo.Repository.ModelsRepository.OrderItemRepository;
using Demo.Repository.ModelsRepository.OrderModel;
using Demo.Repository.ModelsRepository.ProductModel;
using Demo.Repository.ModelsRepository.StaffModel;
using Demo.Repository.ModelsRepository.StockModel;
using Demo.Repository.ModelsRepository.StoreModel;
using Microsoft.EntityFrameworkCore;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IunitOfWork,unitOfWork>();
            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
