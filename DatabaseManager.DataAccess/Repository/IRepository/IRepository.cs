using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        int GetRowCount(int shardId);
        int GetRowCount();
        int GetColumnCount(int shardId);
        int GetColumnCount();
        int GetUsedSpace(int shardId);
        int GetUsedSpace();
        IEnumerable<T> GetPagedEntities(int page, int pageSize, int shardId);
        IEnumerable<T> GetPagedEntities(int page, int pageSize);
        List<Guid> GetAllIds(int shardId);
        List<Guid> GetAllIds();
        T? GetById(Guid id, int shardId, string? includeProperties = null);
        T? GetById(Guid id, string? includeProperties = null);
        T? GetSingleOrDefault(Expression<Func<T, bool>> filter, int shardId, string? includeProperties = null);
        T? GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, int shardId, string? includeProperties = null);
        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(int shardId, Expression<Func<T, bool>>? filter = null,  string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        EntityEntry<T> Add(T entity, Guid cSessionId, int shardId = 1);
        void Remove(T entity, int shardId = 1);
        void RemoveRange(IEnumerable<T> entities, int shardId = 1);
    }
}
