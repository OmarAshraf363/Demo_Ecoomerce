﻿using Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        //public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        //public virtual DbSet<Staff> Staffs { get; set; }

        public virtual DbSet<Stock> Stocks { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItems> WishListItems { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
        //    var connection = builder.GetConnectionString("DefaultConnection");

        //    optionsBuilder.UseSqlServer(connection);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderItem>().HasKey(e => new { e.OrderId, e.ItemId });


            modelBuilder.Entity<Stock>()
                 .HasKey(s => new { s.StoreId, s.ProductId });

            modelBuilder.Entity<AppUser>().HasData(
       new AppUser
       {
           Id = "123548458", // User ID should be a string
           UserName = "Admin@gmail.com",
           Email = "Admin@gmail.com",
           NormalizedUserName = "ADMIN@GMAIL.COM",
           NormalizedEmail = "ADMIN@GMAIL.COM",
           EmailConfirmed = false,
           PasswordHash = new PasswordHasher<AppUser>().HashPassword(null, "Admin@123"),
           SecurityStamp = Guid.NewGuid().ToString(), // Ensure this is unique
           Address = "Test"
       }
   );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1", // Role ID should be a string
                    Name = Check.Methods.StaticData_AdminRole,
                    NormalizedName = Check.Methods.StaticData_AdminRole.ToUpper()
                },
                new IdentityRole
                {
                    Id = "2", // Role ID should be a string
                    Name = Check.Methods.StaticData_CustomerRole,
                    NormalizedName = Check.Methods.StaticData_CustomerRole.ToUpper()
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1", // Role ID as string
                    UserId = "123548458" // User ID as string
                }
            );


            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.HasOne(e => e.AppUser)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(e => e.AppUserId)
                      .OnDelete(DeleteBehavior.NoAction);



                entity.HasOne(e => e.Store)
                      .WithMany(st => st.Orders)
                      .HasForeignKey(e => e.StoreId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            var categories = new List<Category>()
                {
                        new Category { CategoryId=2,CategoryName = "Electronics" },
                        new Category { CategoryId=3,CategoryName = "Clothing" },
                        new Category {CategoryId=4 ,CategoryName = "Books" },
                };
            modelBuilder.Entity<Category>().HasData(categories);

            var brands = new List<Brand>()
            {
                new Brand { BrandId=2,BrandName = "Brand A" },
                new Brand { BrandId=3,BrandName = "Brand B" },
                new Brand { BrandId=4,BrandName = "Brand C" },
            };
            modelBuilder.Entity<Brand>().HasData(brands);

            var products = new List<Product>()
            {
                 new Product
        {
                     ProductId=3,
            ProductName = "Smartphone",
            ProductDescription = "Latest model with advanced features.",
            Rate = 5,
            Image = "5.png",
            BrandId = brands.First(b => b.BrandName == "Brand A").BrandId,
            CategoryId = categories.First(c => c.CategoryName == "Electronics").CategoryId,
            ModelYear = 2023,
            ListPrice = 999.99m
        },
        new Product
        {
            ProductId=4,
            ProductName = "Jeans",
            ProductDescription = "Comfortable and stylish jeans.",
            Rate = 4,
            Image = "jeans.jpg",
            BrandId = brands.First(b => b.BrandName == "Brand B").BrandId,
            CategoryId = categories.First(c => c.CategoryName == "Clothing").CategoryId,
            ModelYear = 2022,
            ListPrice = 49.99m
        },
        new Product
        {
            ProductId = 5,
            ProductName = "Novel",
            ProductDescription = "Bestselling novel by a famous author.",
            Rate = 4,
            Image = "Book.jpeg",
            BrandId = brands.First(b => b.BrandName == "Brand C").BrandId,
            CategoryId = categories.First(c => c.CategoryName == "Books").CategoryId,
            ModelYear = 2021,
            ListPrice = 19.99m
        }
            };
            modelBuilder.Entity<Product>().HasData(products);
        }

    }
}
