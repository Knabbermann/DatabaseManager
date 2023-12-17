using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DatabaseManager.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        int GetRowCount(int shardId = 1);
        int GetColumnCount(int shardId = 1);
        int GetUsedSpace(int shardId = 1);
        IEnumerable<T> GetPagedEntities(int page, int pageSize, int shardId = 1);
        List<int> GetAllIds(int shardId = 1);
        T? GetById(int id, string? includeProperties = null, int shardId = 1);
        T? GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, int shardId = 1);
        T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, int shardId = 1);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int shardId = 1);
        EntityEntry<T> Add(T entity, Guid cSessionId, int shardId = 1);
        void Remove(T entity, int shardId = 1);
        void RemoveRange(IEnumerable<T> entities, int shardId = 1);
    }
}
