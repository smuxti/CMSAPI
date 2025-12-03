using Merchants.Core.Common;
using System.Linq.Expressions;

namespace Merchants.Core.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T,bool>> predicate);
        Task<T> GetById(int Id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteHardAsync(T entity);
        Task<T> GetByGuidId(Guid Id);
    }
}
