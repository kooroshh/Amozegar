using System.Linq.Expressions;

namespace Amozegar.Data.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderby = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null
        );
        Task<TEntity?> GetByIdAsync(object key);
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);
        Task DeleteByIdAsync(object key);
        void Update(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? where = null);
    }
}
