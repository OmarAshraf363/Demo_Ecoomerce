using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repository.IRepository
{
    public class GenralRepository<T>  : IGenralRepository<T> where T : class 
    {
        private readonly AppDbContext context;
        private readonly DbSet<T> dbSet;

        public GenralRepository(AppDbContext context) 
        {
            this.context = context;
            dbSet=context.Set<T>();
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public IEnumerable<T> GetAll()
        {
           return dbSet.AsQueryable();
        }

        public T? GetById(int id)
        {
            return dbSet.Find(id);
            
        }
        public void Create(T entity)
        {
            dbSet.Add(entity);
            Save();
        }

        public void Delete(int id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                Save();
            }
        }

        public void Edit(T entity)
        {
            dbSet.Update(entity);
            Save();
        }


    }
}
