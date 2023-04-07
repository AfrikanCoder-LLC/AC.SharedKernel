
namespace Kernel.Interfaces
{
    public interface IGenericReferenceEntityRepository<T> : IGenericRepository<T> where T : ReferenceEntity
    {
        Task<T> GetMatchingStaticEntityFromDb(T entity);
    }
}
