using System.Linq.Expressions;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface ILogRepository<T> where T : class
    {
        int GetRowCount();
        int GetUsedSpace();
        public IEnumerable<T> GetPagedEntities<TKey>(Expression<Func<T, TKey>> orderByExp, int page, int pageSize);
        T? GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
