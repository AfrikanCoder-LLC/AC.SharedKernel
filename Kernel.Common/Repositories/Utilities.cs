using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Common.Repositories
{
    public static class Utilities
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void FixState(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<IObjectState>())
            {
                IObjectState stateInfo = entry.Entity;
                entry.State = ConvertState(stateInfo.State);
            }
        }

        private static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.ADDED:
                    return EntityState.Added;

                case ObjectState.MODIFIED:
                    return EntityState.Modified;

                case ObjectState.REMOVED:
                    return EntityState.Deleted;

                default:
                    return EntityState.Unchanged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(int id)
        {
            var item = Expression.Parameter(typeof(TEntity), "entity");
            var prop = Expression.Property(item, "Id");
            var value = Expression.Constant(id);
            var equal = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
            return lambda;
        }
    }
}
