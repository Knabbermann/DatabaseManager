using System.Linq.Expressions;
using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DatabaseManager.DataAccess.Repository
{
    public class Repository<T>(WebDbContextShard1 webDbContextShard1, WebDbContextShard2 webDbContextShard2, IUnitOfWork unitOfWork) : IRepository<T> where T : class, IEntity
    {
        internal DbSet<T> DbSetShard1 = webDbContextShard1.Set<T>();
        internal DbSet<T> DbSetShard2 = webDbContextShard2.Set<T>();

        private DbSet<T> GetCurrentDbShard(int shardId)
        {
            if (shardId == 1) return DbSetShard1;
            if (shardId == 2) return DbSetShard2;
            throw new ArgumentException("ShardId not known.");
        }

        public int GetRowCount(int shardId = 1)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            return query.Count();
        }

        public int GetColumnCount(int shardId = 1)
        {
            using var dbConnection = unitOfWork.GetDbConnection(shardId);
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@TableName";
            parameter.Value = GetCurrentDbShard(shardId).EntityType.Name.Split('.')[^1] + "s";
            command.Parameters.Add(parameter);
            var columnCount = Convert.ToInt32(command.ExecuteScalar());
            dbConnection.Close();
            return columnCount;
        }

        public int GetUsedSpace(int shardId = 1)
        {
            using var dbConnection = unitOfWork.GetDbConnection(shardId);
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = @"
                SELECT SUM(a.used_pages) * 8
                FROM sys.tables t
                INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
                INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
                WHERE t.NAME = @TableName";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@TableName";
            parameter.Value = GetCurrentDbShard(shardId).EntityType.Name.Split('.')[^1] + "s";
            command.Parameters.Add(parameter);
            var usedSpace = Convert.ToInt32(command.ExecuteScalar());
            dbConnection.Close();
            return usedSpace;
        }

        public IEnumerable<T> GetPagedEntities(int page, int pageSize, int shardId = 1)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            return query.OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<int> GetAllIds(int shardId = 1)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            return query.Select(x => x.Id)
                .ToList();
        }

        public T? GetById(int id, string? includeProperties = null, int shardId = 1)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            query = query.Where(entity => entity.Id.Equals(id));
            return query.SingleOrDefault();
        }

        public T? GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, int shardId = 1)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            query = query.Where(filter);

            return query.SingleOrDefault();
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, int shardId = 1)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int shardId = 1)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            if (filter is not null) query = query.Where(filter);
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return query.ToList();
        }

        public EntityEntry<T> Add(T entity, Guid cSessionId, int shardId = 1)
        {
            var uEntity = GetCurrentDbShard(shardId).Add(entity);
            unitOfWork.SaveChanges(shardId);
            unitOfWork.LogWithId.Add(new LogWithId { Model = uEntity.Entity.ToString().Split('.')[2], ModelId = uEntity.Entity.Id, SessionGuid = cSessionId });
            unitOfWork.LogWithGuid.Add(new LogWithGuid { Model = uEntity.Entity.ToString().Split('.')[2], ModelId = uEntity.Entity.Id, SessionGuid = cSessionId });
            unitOfWork.SaveChanges();
            return uEntity;
        }

        public void Remove(T entity, int shardId = 1)
        {
            GetCurrentDbShard(shardId).Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities, int shardId = 1)
        {
            GetCurrentDbShard(shardId).RemoveRange(entities);
        }
    }
}
