using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected AmozegarContext _context;
        protected DbSet<TEntity> _dbSet;

        public GenericRepository(AmozegarContext context)
        {
            this._context = context;
            this._dbSet = this._context.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await this._dbSet.AddAsync(entity);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where)
        {
            return await this._dbSet.AnyAsync(where);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? where = null)
        {
            return where == null ? await this._dbSet.CountAsync() : await this._dbSet.CountAsync(where);
        }

        public virtual void Delete(TEntity entity)
        {
            this._dbSet.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            this._dbSet.RemoveRange(entities);
        }

        public virtual async Task DeleteByIdAsync(object key)
        {
            var entity = await this.GetByIdAsync(key);
            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderby = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = this._dbSet;

            if (include != null)
            {
                query = include(query);
            }


            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderby != null)
            {
                query = orderby(query);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(object key)
        {
            var entity = await this._dbSet.FindAsync(key);
            return entity;
        }

        public virtual void Update(TEntity entity)
        {
            this._dbSet.Update(entity);
        }
    }
}
