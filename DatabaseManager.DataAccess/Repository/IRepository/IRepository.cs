using System.Linq.Expressions;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        int GetRowCount();
        int GetColumnCount();
        int GetUsedSpace();
        public IEnumerable<T> GetPagedEntities(int page, int pageSize);
        T? GetById(int id, string? includeProperties = null);
        T? GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
