
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VoterDeatils.DataAccess;

namespace VoterDetails.DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext Context;
        internal DbSet<TEntity> dbSet;
        public GenericRepository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            dbSet = context.Set<TEntity>();
        }
        #region Synchronous Methods
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Expression<Func<TEntity, bool>> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IEnumerable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Expression<Func<TEntity, bool>> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, string orderByColumn, bool sortAscending = true,
                                                Expression<Func<TEntity, bool>> includeProperties = null)
        {
            IQueryable<TEntity> query = this.dbSet;

            query = query.Where(filter);

            if (includeProperties != null)
            {
                query = query.Include(includeProperties);
            }

            return query.OrderByExtension(orderByColumn, sortAscending);
        }

        public virtual TEntity GetById(Expression<Func<TEntity, bool>> id)
        {
            return dbSet.FirstOrDefault(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void InsertAll(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities?.Where(a => a != null))
            {
                Add(entity);
            }
        }
        /// <summary>
        /// Additional Method for Add
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity)
        {
            var dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                dbSet.Add(entity);
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }
        /// <summary>
        /// Delete an entity by passing an ID
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(Expression<Func<TEntity, bool>> id)
        {
            TEntity entityToDelete = dbSet.FirstOrDefault(id);
            if (entityToDelete != null)
                Delete(entityToDelete);
        }

        public async Task DeleteAll(Expression<Func<TEntity, bool>> queryToDelete)
        {
            IEnumerable<TEntity> entitiesToDelete = await dbSet.Where(queryToDelete).ToListAsync();
            foreach (var item in entitiesToDelete)
            {
                if (item != null)
                    Delete(item);
            }
        }

        /// <summary>
        /// Delete an Entity from a Model
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }
        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Update(entityToUpdate);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entitiesToUpdate)
        {
            dbSet.UpdateRange(entitiesToUpdate);
        }
        /// <summary>
        /// Retrieves all the entity from a model
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> All()
        {
            return this.dbSet.AsEnumerable();
        }
        /// <summary>
        /// Retrieves all the entity from a model
        /// </summary>
        /// <param name="tracking"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll(bool tracking = true)
        {
            if (tracking)
                return this.dbSet.AsEnumerable(); //Checking with AsExpandable
            else
                return this.dbSet
                      .AsNoTracking()
                     .AsEnumerable();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKProperty"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetPaged<TKProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending,
                                                                bool tracking = true)
        {
            var set = this.dbSet;

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsEnumerable();
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount)
                          .AsEnumerable();
            }
        }



        public virtual IEnumerable<TEntity> GetFilterPaged<TKProperty>(int pageIndex, int pageCount, int skip, Expression<Func<TEntity, bool>> filterExpression,
                                                                      Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending = true, bool tracking = true)
        {
            var set = this.dbSet;

            if (ascending)
            {
                return set.Where(filterExpression)
                          .OrderBy(orderByExpression)
                          .Skip(skip)
                          .Take(pageCount)
                          .AsEnumerable();
            }
            else
            {
                return set.Where(filterExpression)
                          .OrderByDescending(orderByExpression)
                          .Skip(skip)
                          .Take(pageCount)
                          .AsEnumerable();
            }
        }

        public virtual Task<List<TEntity>> GetFilterPaged<TKProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filterExpression,
                                                                      Expression<Func<TEntity, TKProperty>> orderByExpression, out int noOfRecords, bool ascending = true, bool tracking = true,
                                                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            int skip = (pageIndex - 1) * pageCount;
            noOfRecords = query.Count(filterExpression);

            if (ascending)
            {
                return query.Where(filterExpression)
                          .OrderBy(orderByExpression)
                          .Skip(skip)
                          .Take(pageCount)
                          .ToListAsync();
            }
            else
            {
                return query.Where(filterExpression)
                          .OrderByDescending(orderByExpression)
                          .Skip(skip)
                          .Take(pageCount)
                          .ToListAsync();
            }
        }

        public virtual Task<List<TEntity>> GetFilterData(Expression<Func<TEntity, bool>> filterExpression,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null)
        {
            IQueryable<TEntity> query = this.dbSet;
            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            return query.Where(filterExpression).ToListAsync();
            //.OrderBy(orderByExpression)
        }

        public virtual IEnumerable<TEntity> GetFilterPaged(int pageSize, int skip, Expression<Func<TEntity, bool>> filterExpression, string orderByColumn,
                                                           ref int rowCount, bool ascending = true, bool tracking = true)
        {
            var set = this.dbSet;
            rowCount = set.Count();
            return set.Where(filterExpression)
                      .OrderByExtension(orderByColumn, ascending)
                      .Skip(skip)
                      .Take(pageSize)
                      .AsEnumerable();
        }

        public virtual IAsyncEnumerable<TEntity> GetFilterPagedQuery(int pageSize, int skip, Expression<Func<TEntity, bool>> filterExpression,
                                   ref int rowCount, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;
            rowCount = query.Count();

            query = query.Where(filterExpression);

            if (orderBy != null)
            {
                query = orderBy(query);
                return query.Skip(skip)
                            .Take(pageSize)
                            .AsNoTracking()
                            .AsAsyncEnumerable();
            }
            else
                return query.Where(filterExpression)
                          .Skip(skip)
                          .Take(pageSize)
                          .AsNoTracking()
                          .AsAsyncEnumerable();

        }

        public virtual IEnumerable<TEntity> GetPagedData(Expression<Func<TEntity, bool>> searchExpression, ref int rowsCount,
                                                         Expression<Func<TEntity, bool>> orderByExpression = null)
        {
            IQueryable<TEntity> query = dbSet;
            rowsCount = query.Count();
            query = query.Where(searchExpression);
            return orderByExpression != null ? query.OrderBy(orderByExpression) : query;
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        /// <summary>
        /// GenericRepository class to provide additional filtering and sorting flexibility without requiring that you create a derived class with additional methods
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        //public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        //{
        //    return dbSet.FromSql(query, parameters).AsEnumerable();
        //}



        #endregion

        #region Asynchronous Methods
        public virtual IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
                                                          IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
                return orderBy(query).AsNoTracking().AsAsyncEnumerable();
            else
                return query.AsNoTracking().AsAsyncEnumerable();
        }

        public virtual IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                return orderBy(query).AsNoTracking().AsAsyncEnumerable();
            else
                return query.AsNoTracking().AsAsyncEnumerable();
        }

        public virtual IQueryable<TEntity> GetAsyncView(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                return orderBy(query);
            else
                return query;
        }

        public virtual IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, bool>> includeProperties = null,
                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);
            if (includeProperties != null)
                query = query.Include(includeProperties);
            if (orderBy != null)
                return orderBy(query).AsAsyncEnumerable();
            else
                return query.AsAsyncEnumerable();
        }


        //public virtual IQueryable<TEntity> GetFromSql(RawSqlString sql, params object[] parameters)
        //{
        //    IQueryable<TEntity> query = dbSet;
        //    if (parameters != null && parameters.Any())
        //        return query.FromSql(sql, parameters); //Context.Attach<TEntity>(entity).Context.Database.get
        //    else
        //        return query.FromSql(sql);
        //}

        public virtual IQueryable<TEntity> GetQueryableAsync(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                return orderBy(query);
            else
                return query;
        }

        public virtual TType MaxBy<TType>(Expression<Func<TEntity, TType>> maxByExpression)
        {
            IQueryable<TEntity> query = dbSet;
            var result = query.FirstOrDefault() != null ? query.Max(maxByExpression) : default;
            return result == null ? default : result;
        }

        /// <summary>
        /// Retrieves all the entity from a model
        /// </summary>
        /// <param name="tracking"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAllAsync(bool tracking = false)
        {
            var set = this.dbSet;
            if (tracking)
            {
                return await set.ToListAsync();
            }
            else
            {
                return await set.AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        /// Async Find
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IAsyncEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;
            if (tracking)
                return query.Where(predicate).AsAsyncEnumerable();
            else
                return query.Where(predicate).AsNoTracking().AsAsyncEnumerable();
        }

        /// <summary>
        /// Async Get
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderByColumn"></param>
        /// <param name="sortAscending"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string orderByColumn, bool sortAscending = true,
                                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null)
        {
            IQueryable<TEntity> query = this.dbSet;

            query = query.Where(filter);

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            return query.OrderByExtension(orderByColumn, sortAscending).AsNoTracking().AsAsyncEnumerable();
        }

        public virtual Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this.dbSet;
            if (includeProperties != null)
            {
                foreach (var includeObj in includeProperties)
                {
                    query = query.Include(includeObj);
                }
            }
            return query.FirstOrDefaultAsync(filter);
        }


        public virtual Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>> includeProperties)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }


            return query.FirstOrDefaultAsync(filter);
        }

        public IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string orderByColumn, bool sortAscending = true, Expression<Func<TEntity, bool>> includeProperties = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
