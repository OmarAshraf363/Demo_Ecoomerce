using Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

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
        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public T? GetOne(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties)
        {
            return Get(expression, includeProperties).FirstOrDefault();
        }
        public void Create(T entity)
        {
            dbSet.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
          
                dbSet.Remove(entity);
                Save();
            
        }

        public void Edit(T entity)
        {
            dbSet.Update(entity);
            Save();
        }


    }
}
