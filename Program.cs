using Demo.Data;
using Demo.Models;
using Demo.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
        StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultUI().AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(14);
            options.Lockout.MaxFailedAccessAttempts = 5;





            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });


        // Add other services
        builder.Services.AddScoped<IunitOfWork, unitOfWork>();
        builder.Services.AddSession();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        app.MapRazorPages();
        app.MapControllerRoute(
            name: "default",
            pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
