﻿using System.Linq.Expressions;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseManager.DataAccess.Repository
{
    public class Repository<T>(Microsoft.EntityFrameworkCore.DbContext webDbContext, IUnitOfWork unitOfWork) : IRepository<T> where T : class, IEntity
    {
        internal DbSet<T> DbSet = webDbContext.Set<T>();

        public int GetRowCount()
        {
            IQueryable<T> query = DbSet;
            return query.Count();
        }

        public int GetColumnCount()
        {
            using var dbConnection = unitOfWork.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = @"
                SELECT COUNT(*)
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@TableName";
            parameter.Value = DbSet.EntityType.Name.Split('.')[^1] + "s";
            command.Parameters.Add(parameter);
            var columnCount = Convert.ToInt32(command.ExecuteScalar());
            dbConnection.Close();
            return columnCount;
        }

        public int GetUsedSpace()
        {
            using var dbConnection = unitOfWork.GetDbConnection();
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
            parameter.Value = DbSet.EntityType.Name.Split('.')[^1] + "s";
            command.Parameters.Add(parameter);
            var usedSpace = Convert.ToInt32(command.ExecuteScalar());
            dbConnection.Close();
            return usedSpace;
        }

        public T? GetById(int id, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            query = query.Where(entity => entity.Id.Equals(id));
            return query.SingleOrDefault();
        }

        public T? GetSingleOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            query = query.Where(filter);

            return query.SingleOrDefault();
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter is not null) query = query.Where(filter);
            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return query.ToList();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }
    }
}
