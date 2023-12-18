using System.Data;
using System.Data.Common;
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

        public int GetRowCount(int shardId)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            return query.Count();
        }

        public int GetRowCount()
        {
            IQueryable<T> queryShard1 = DbSetShard1;
            IQueryable<T> queryShard2 = DbSetShard2;
            return queryShard1.Count() + queryShard2.Count();
        }

        public int GetColumnCount(int shardId)
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

        public int GetColumnCount()
        {
            var columnCount = 0;
            var tableName = DbSetShard1.EntityType.Name.Split('.')[^1] + "s";

            using (var dbConnectionShard1 = unitOfWork.GetDbConnection(1))
            {
                dbConnectionShard1.Open();
                columnCount = GetColumnCountForTable(dbConnectionShard1, tableName);
            }

            using (var dbConnectionShard2 = unitOfWork.GetDbConnection(2))
            {
                dbConnectionShard2.Open();
                if(GetColumnCountForTable(dbConnectionShard2, tableName) != columnCount) columnCount = -1;
            }

            return columnCount;
        }

        public int GetUsedSpace(int shardId)
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

        public int GetUsedSpace()
        {
            var usedSpace = 0;
            var tableName = DbSetShard1.EntityType.Name.Split('.')[^1] + "s";

            using (var dbConnectionShard1 = unitOfWork.GetDbConnection(1))
            {
                dbConnectionShard1.Open();
                usedSpace += GetUsedSpaceForTable(dbConnectionShard1, tableName);
            }

            using (var dbConnectionShard2 = unitOfWork.GetDbConnection(2))
            {
                dbConnectionShard2.Open();
                usedSpace += GetUsedSpaceForTable(dbConnectionShard2, tableName);
            }

            return usedSpace;
        }

        public IEnumerable<T> GetPagedEntities(int page, int pageSize, int shardId)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            return query.OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<T> GetPagedEntities(int page, int pageSize)
        {
            IQueryable<T> queryShard1 = DbSetShard1;
            IQueryable<T> queryShard2 = DbSetShard2;

            var query = queryShard1.OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            query.AddRange(queryShard2.OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList());
            return query;
        }

        public List<Guid> GetAllIds(int shardId)
        {
            IQueryable<T> query = GetCurrentDbShard(shardId);
            return query.Select(x => x.Id)
                .ToList();
        }

        public List<Guid> GetAllIds()
        {
            IQueryable<T> queryShard1 = DbSetShard1;
            IQueryable<T> queryShard2 = DbSetShard2;
            var query = queryShard1.Select(x => x.Id)
                .ToList();
            query.AddRange(queryShard2.Select(x => x.Id)
                .ToList());
            return query;
        }

        public T? GetById(Guid id, int shardId, string? includeProperties = null)
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

        public T? GetById(Guid id, string? includeProperties = null)
        {
            var queryShard1 = ApplyIncludeProperties(DbSetShard1, includeProperties);
            var queryShard2 = ApplyIncludeProperties(DbSetShard2, includeProperties);

            var entity = queryShard1.FirstOrDefault(entity => entity.Id.Equals(id));

            return entity ?? queryShard2.FirstOrDefault(entity => entity.Id.Equals(id));
        }

        public T? GetSingleOrDefault(Expression<Func<T, bool>> filter, int shardId, string? includeProperties = null)
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

        public T? GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            var queryShard1 = ApplyIncludeProperties(DbSetShard1, includeProperties);
            var queryShard2 = ApplyIncludeProperties(DbSetShard2, includeProperties);

            var entity = queryShard1.Where(filter).SingleOrDefault();

            return entity ?? queryShard2.Where(filter).SingleOrDefault();
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, int shardId, string? includeProperties = null)
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

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            var queryShard1 = ApplyIncludeProperties(DbSetShard1, includeProperties);
            var queryShard2 = ApplyIncludeProperties(DbSetShard2, includeProperties);

            var entity = queryShard1.Where(filter).FirstOrDefault();

            return entity ?? queryShard2.Where(filter).FirstOrDefault();
        }

        public IEnumerable<T> GetAll(int shardId, Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
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

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            var queryShard1 = ApplyIncludeProperties(DbSetShard1, includeProperties);
            var queryShard2 = ApplyIncludeProperties(DbSetShard2, includeProperties);

            var entity = queryShard1.ToList();
            entity.AddRange(queryShard2.ToList());
            return entity;
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

        private static int GetColumnCountForTable(IDbConnection dbConnection, string tableName)
        {
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName";

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@TableName";
                parameter.Value = tableName;
                command.Parameters.Add(parameter);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static int GetUsedSpaceForTable(IDbConnection dbConnection, string tableName)
        {
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT SUM(a.used_pages) * 8
                    FROM sys.tables t
                    INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
                    INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                    INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
                    WHERE t.NAME = @TableName";

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@TableName";
                parameter.Value = tableName;
                command.Parameters.Add(parameter);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private IQueryable<T> ApplyIncludeProperties(IQueryable<T> query, string? includeProperties)
        {
            if (includeProperties != null)
            {
                return includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return query;
        }
    }
}
