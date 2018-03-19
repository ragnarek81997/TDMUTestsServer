using TDMUTestsServer.Domain.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Interfaces.Infrastructure
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IBaseEntity
    {
        #region COUNT
        Task<DatabaseResult<int>> CountAsync(Expression<Func<TEntity, bool>> where = null);
        #endregion

        #region DELETE
        Task<DatabaseResult> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<DatabaseResult> DeleteOneAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region FIND_MANY
        Task<DatabaseManyResult<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include = null, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null, int limit = 50, int skip = 0, bool isAll = false);
        #endregion

        #region FIND_ONE
        Task<DatabaseOneResult<TEntity>> FindOneAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> include = null, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null);
        #endregion

        #region GET_ALL
        Task<DatabaseManyResult<TEntity>> GetAllWithOrderAscAsync(Expression<Func<TEntity, object>> predicate = null, Expression<Func<TEntity, object>> include = null, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null, int limit = 50, int skip = 0, bool isAll = false);

        Task<DatabaseManyResult<TEntity>> GetAllWithOrderDescAsync(Expression<Func<TEntity, object>> predicate = null, Expression<Func<TEntity, object>> include = null, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null, int limit = 50, int skip = 0, bool isAll = false);
        #endregion

        #region INSERT
        Task<DatabaseResult> InsertManyAsync(List<TEntity> entities);

        Task<DatabaseResult> InsertOneAsync(TEntity entity);
        #endregion

        #region UPDATE
        Task<DatabaseResult> UpdateOneAsync(TEntity entity);
        #endregion

        #region SQL_QUERY
        Task<DatabaseResult> SqlQueryAsync(string query);
        Task<DatabaseResult<int>> SqlQueryIntAsync(string query);

        Task<DatabaseResult<string>> SqlQueryStringAsync(string query);

        Task<DatabaseOneResult<TEntity>> SqlQueryEntityAsync(string query);

        Task<DatabaseManyResult<TEntity>> SqlQueryManyEntitiesAsync(string query);
        #endregion
    }
}
