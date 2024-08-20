namespace Demo.Repository.IRepository
{
    public interface IGenralRepository<T> where T : class
    {
        void Save();
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Edit(T entity);
        void Create(T entity);
        void Delete(int id);

    }
}
