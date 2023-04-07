using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : AggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        IQueryable<TEntity> FullyLoadedQueryable { get; }

        Task<IEnumerable<TEntity>> AllFullyLoaded();

        Task<IEnumerable<TEntity>> AllEagerLoaded();

        Task<IEnumerable<TEntity>> AllInclude
            (params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> FindFullyLoaded
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> FindFullyLoadedBy(Expression<Func<TEntity, bool>> predicate, int? recordLimit = null);
        Task<ICollection<string>> ValidateKey(int? id, string fieldName, bool allowNull);
        Task<TEntity> FindFullyLoadedByKey(int id, bool throwExceptionIfNotFound = false);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(IEnumerable<TEntity> entity);

        Task DeleteAsync(int id);

        Task LoadRelated(Entity entity);
        Task LoadRelatedAsync(IEnumerable<Entity> entities);

        /// <summary>
        /// Load the entire aggregate so we are ready for processing as a domain object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task LoadAggregateAsync(TEntity entity);
        Task LoadAggregateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Load the entire child aggregate
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task LoadChildAggregateAsync(Entity entity);
        Task LoadChildAggregateAsync(IEnumerable<Entity> entities);
    }
}
