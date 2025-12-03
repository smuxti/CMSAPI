using Merchants.Core.Common;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Merchants.Infrastructure.Repositories
{

    public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly MerchantContext _dbContext;
        protected readonly ILogger _logger;
        public AsyncRepository(MerchantContext merchantContext, ILogger<AsyncRepository<T>> logger)
        {
            _dbContext = merchantContext;
            _logger = logger;
        }
        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            entity.isDeleted = true;
            entity.Status = "InActive";
            //entity.DeletedBy = Guid.Parse("0A7EB5DF-B507-4620-938A-E7CB493B465A");
            entity.DeletedAt=DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteHardAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetById(int Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        public async Task<T> GetByGuidId(Guid Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
