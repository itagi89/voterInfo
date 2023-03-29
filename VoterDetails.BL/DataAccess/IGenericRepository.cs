using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VoterDetails.DataAccess
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// this will be used for retreving the list of records for an expression
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        #region Synchronous Methods
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
                                    IOrderedQueryable<TEntity>> orderBy = null,
                                 Expression<Func<TEntity, bool>> includeProperties = null);

        IEnumerable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Expression<Func<TEntity, bool>> includeProperties = null);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, string orderByColumn, bool sortAscending = true, Expression<Func<TEntity, bool>> includeProperties = null);

        TEntity GetById(Expression<Func<TEntity, bool>> id);
        void Insert(TEntity entity);
        void InsertAll(IEnumerable<TEntity> entities);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Delete(Expression<Func<TEntity, bool>> id);
        Task DeleteAll(Expression<Func<TEntity, bool>> queryToDelete);
        void Delete(TEntity entityToDelete);
        void DeleteRange(IEnumerable<TEntity> entities);
        void Update(TEntity entityToUpdate);
        void UpdateRange(IEnumerable<TEntity> entitiesToUpdate);
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> GetAll(bool tracking = true);
        IEnumerable<TEntity> GetPaged<TKProperty>(int pageIndex, int pageCount,
                                      Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending, bool tracking = true);
        IEnumerable<TEntity> GetFilterPaged<TKProperty>(int pageIndex, int pageCount, int skip,
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending = true, bool tracking = true);

        Task<List<TEntity>> GetFilterPaged<TKProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filterExpression,
                                                                      Expression<Func<TEntity, TKProperty>> orderByExpression, out int noOfRecords, bool ascending = true, bool tracking = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null);

        IEnumerable<TEntity> GetPagedData(Expression<Func<TEntity, bool>> searchExpression, ref int rowsCount, Expression<Func<TEntity, bool>> orderByExpression = null);
        IEnumerable<TEntity> GetFilterPaged(int pageSize, int skip, Expression<Func<TEntity, bool>> filterExpression, string orderByColumn, ref int rowCount, bool ascending = true, bool tracking = true);
        //IQueryable<TEntity> GetFilterPagedQuery(int pageSize, int skip, Expression<Func<TEntity, bool>> filterExpression, ref int rowCount);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        //IEnumerable<TEntity> GetWithRawSql(String procedureCommand, params object[] sqlParams);
        //IQueryable<TEntity> GetFromSql(RawSqlString sql, params object[] parameters);
        //void InsertData<TEntity>(List<TEntity> list, string TableName, System.Data.SqlClient.SqlConnection conn);
        //System.Data.DataTable ConvertToDataTable<T>(IList<T> data);
        #endregion

        #region Asynchronous Methods
        IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
         params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> GetAllAsync(bool tracking = false);
        IAsyncEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false);
        IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string orderByColumn, bool sortAscending = true, Expression<Func<TEntity, bool>> includeProperties = null);
        Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null);
        IQueryable<TEntity> GetAsyncView(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IQueryable<TEntity> GetQueryableAsync(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        TType MaxBy<TType>(Expression<Func<TEntity, TType>> maxByExpression);
        IAsyncEnumerable<TEntity> GetFilterPagedQuery(int pageSize, int skip, Expression<Func<TEntity, bool>> filterExpression,
                                    ref int rowCount, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        #endregion
    }
}
