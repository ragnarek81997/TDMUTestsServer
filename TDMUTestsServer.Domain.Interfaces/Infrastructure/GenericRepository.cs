using TDMUTestsServer.Domain.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Interfaces.Infrastructure
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private ApplicationDbContext _applicationDbContext = null;

        public ApplicationDbContext ApplicationDbContex
        {
            get
            {
                return _applicationDbContext;
            }
        }

        public GenericRepository()
        {
            _applicationDbContext = new ApplicationDbContext();
        }

        public GenericRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        #region COUNT
        public async Task<DatabaseResult<int>> CountAsync(Expression<Func<TEntity, bool>> where = null)
        {
            var dbResult = new DatabaseResult<int>();
            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();

                var result = await (where == null ?  dbSet.CountAsync() : dbSet.CountAsync(where));

                dbResult.Success = true;
                dbResult.Entity = result;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception CountAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        #region DELETE
        public async Task<DatabaseResult> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var dbResult = new DatabaseResult();

            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();

                IEnumerable<TEntity> objects = dbSet.Where<TEntity>(predicate ?? (_ => true)).AsEnumerable();

                foreach (TEntity obj in objects)
                    dbSet.Remove(obj);

                await _applicationDbContext.SaveChangesAsync();

                dbResult.Success = true;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception DeleteManyAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }

        public async Task<DatabaseResult> DeleteOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var dbResult = new DatabaseResult();

            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();

                IEnumerable<TEntity> objects = dbSet.Where<TEntity>(predicate).AsEnumerable();

                foreach (TEntity obj in objects)
                {
                    dbSet.Remove(obj);
                    break;
                }

                await _applicationDbContext.SaveChangesAsync();

                dbResult.Success = true;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception DeleteManyAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        #region FIND_MANY
        public async Task<DatabaseManyResult<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> include = null, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null, int limit = 50, int skip = 0, bool isAll = false)
        {
            var dbResult = new DatabaseManyResult<TEntity>();
            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();

                var dbQuery = dbSet.AsQueryable();
                dbQuery = (include != null) ? dbQuery.Include(include) : dbQuery;
                dbQuery = (include2 != null) ? dbQuery.Include(include2) : dbQuery;
                dbQuery = (include3 != null) ? dbQuery.Include(include3) : dbQuery;
                dbQuery = (include4 != null) ? dbQuery.Include(include4) : dbQuery;

                var result = dbSet.Where(predicate).OrderBy(_ => _.Id);
                var resultQuery = !isAll ? result.Skip(skip).Take(limit) : result;
                var resultList = await resultQuery.ToListAsync<TEntity>();

                if (result == null) throw new ArgumentNullException("Result object is null.");

                dbResult.Success = true;
                dbResult.Entities = resultList;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception FindManyAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        #region FIND_ONE
        public async Task<DatabaseOneResult<TEntity>> FindOneAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> include = null, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null)
        {
            var dbResult = new DatabaseOneResult<TEntity>();
            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();

                var dbQuery = dbSet.AsQueryable();
                dbQuery = (include != null) ? dbQuery.Include(include) : dbQuery;
                dbQuery = (include2 != null) ? dbQuery.Include(include2) : dbQuery;
                dbQuery = (include3 != null) ? dbQuery.Include(include3) : dbQuery;
                dbQuery = (include4 != null) ? dbQuery.Include(include4) : dbQuery;

                var result = await dbQuery.FirstOrDefaultAsync(where);

                if (result == null) throw new ArgumentNullException("Result object is null.");

                dbResult.Success = true;
                dbResult.Entity = result;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception FindOneAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        #region GET_ALL
        public async Task<DatabaseManyResult<TEntity>> GetAllWithOrderAscAsync(Expression<Func<TEntity, object>> predicate = null, Expression<Func<TEntity, object>> include = null, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null, int limit = 50, int skip = 0, bool isAll = false)
        {
            var dbResult = new DatabaseManyResult<TEntity>();
            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();

                var dbQuery = dbSet.AsQueryable();
                dbQuery = (include != null) ? dbQuery.Include(include) : dbQuery;
                dbQuery = (include2 != null) ? dbQuery.Include(include2) : dbQuery;
                dbQuery = (include3 != null) ? dbQuery.Include(include3) : dbQuery;
                dbQuery = (include4 != null) ? dbQuery.Include(include4) : dbQuery;

                var result = dbSet.OrderBy( predicate ?? (_ => _.Id));
                var resultQuery = !isAll ? result.Skip(skip).Take(limit) : result;
                var resultList = await resultQuery.ToListAsync<TEntity>();

                if (result == null) throw new ArgumentNullException("Result object is null.");

                dbResult.Success = true;
                dbResult.Entities = resultList;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception GetAllAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }

        public async Task<DatabaseManyResult<TEntity>> GetAllWithOrderDescAsync(Expression<Func<TEntity, object>> predicate, Expression<Func<TEntity, object>> include, Expression<Func<TEntity, object>> include2 = null, Expression<Func<TEntity, object>> include3 = null, Expression<Func<TEntity, object>> include4 = null, int limit = 50, int skip = 0, bool isAll = false)
        {
            var dbResult = new DatabaseManyResult<TEntity>();
            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();

                var dbQuery = dbSet.AsQueryable();
                dbQuery = (include != null) ? dbQuery.Include(include) : dbQuery;
                dbQuery = (include2 != null) ? dbQuery.Include(include2) : dbQuery;
                dbQuery = (include3 != null) ? dbQuery.Include(include3) : dbQuery;
                dbQuery = (include4 != null) ? dbQuery.Include(include4) : dbQuery;

                var result = dbSet.OrderByDescending(predicate ?? (_ => _.Id));
                var resultQuery = !isAll ? result.Skip(skip).Take(limit) : result;
                var resultList = await resultQuery.ToListAsync<TEntity>();

                if (result == null) throw new ArgumentNullException("Result object is null.");

                dbResult.Success = true;
                dbResult.Entities = resultList;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception GetAllAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        #region INSERT
        public async Task<DatabaseResult> InsertManyAsync(List<TEntity> entities)
        {
            var dbResult = new DatabaseResult();

            try
            {
                _applicationDbContext.Configuration.AutoDetectChangesEnabled = false;
                _applicationDbContext.Configuration.ValidateOnSaveEnabled = false;

                foreach (TEntity entity in entities)
                    _applicationDbContext.Entry(entity).State = EntityState.Added;
                await _applicationDbContext.SaveChangesAsync();

                _applicationDbContext.Configuration.AutoDetectChangesEnabled = true;
                _applicationDbContext.Configuration.ValidateOnSaveEnabled = true;

                dbResult.Success = true;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception InsertManyAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }

        public async Task<DatabaseResult> InsertOneAsync(TEntity entity)
        {
            var dbResult = new DatabaseResult();

            try
            {
                _applicationDbContext.Entry(entity).State = EntityState.Added;

                await _applicationDbContext.SaveChangesAsync();

                dbResult.Success = true;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception InsertOneAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        #region UPDATE
        public async Task<DatabaseResult> UpdateOneAsync(TEntity entity)
        {
            var dbResult = new DatabaseResult();

            try
            {
                var dbSet = _applicationDbContext.Set<TEntity>();
                dbSet.Attach(entity);
                _applicationDbContext.Entry(entity).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                dbResult.Success = true;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception UpdateOneAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        #region SQL_QUERY
        public async Task<DatabaseResult> SqlQueryAsync(string query)
        {
            var dbResult = new DatabaseResult();
            try
            {
                var result = await _applicationDbContext.Database.ExecuteSqlCommandAsync(query);

                dbResult.Success = result >= 0;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception SqlQueryAsync " + typeof(int).Name;
                return dbResult;
            }
        }
        public async Task<DatabaseResult<int>> SqlQueryIntAsync(string query)
        {
            var dbResult = new DatabaseResult<int>();
            try
            {
                var result = await _applicationDbContext.Database.SqlQuery<int>(query).FirstOrDefaultAsync();

                dbResult.Success = true;
                dbResult.Entity = result;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception SqlQueryIntAsync " + typeof(int).Name;
                return dbResult;
            }
        }

        public async Task<DatabaseResult<string>> SqlQueryStringAsync(string query)
        {
            var dbResult = new DatabaseResult<string>();
            try
            {
                var result = await _applicationDbContext.Database.SqlQuery<string>(query).FirstOrDefaultAsync();

                if (result == null) throw new ArgumentNullException("Result object is null.");

                dbResult.Success = true;
                dbResult.Entity = result;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception SqlQueryStringAsync " + typeof(string).Name;
                return dbResult;
            }
        }

        public async Task<DatabaseOneResult<TEntity>> SqlQueryEntityAsync(string query)
        {
            var dbResult = new DatabaseOneResult<TEntity>();
            try
            {
                var result = await _applicationDbContext.Database.SqlQuery<TEntity>(query).FirstOrDefaultAsync();

                if (result == null) throw new ArgumentNullException("Result object is null.");

                dbResult.Success = true;
                dbResult.Entity = result;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception SqlQueryEntityAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }

        public async Task<DatabaseManyResult<TEntity>> SqlQueryManyEntitiesAsync(string query)
        {
            var dbResult = new DatabaseManyResult<TEntity>();
            try
            {
                var result = await _applicationDbContext.Database.SqlQuery<TEntity>(query).ToListAsync();

                if (result == null) throw new ArgumentNullException("Result object is null.");

                dbResult.Success = true;
                dbResult.Entities = result;
                return dbResult;
            }
            catch (Exception ex)
            {
                dbResult.Exception = ex;
                dbResult.Message = "Exception SqlQueryManyEntitiesAsync " + typeof(TEntity).Name;
                return dbResult;
            }
        }
        #endregion

        public void Dispose()
        {
            _applicationDbContext?.Dispose();
        }
    }
}
