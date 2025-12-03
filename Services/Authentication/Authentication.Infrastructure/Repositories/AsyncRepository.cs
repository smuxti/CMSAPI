using Authentication.Core.Common;
using Authentication.Core.Interfaces;
using Authentication.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Authentication.Infrastructure.Repositories
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly AuthenticationContext _dbContext;
        protected readonly ILogger _logger;
        //private readonly AuthorizationFilterContext _context;
        //private readonly string _userid;

        public AsyncRepository(AuthenticationContext dBcontext, ILogger<AsyncRepository<T>> logger)
        {
            _dbContext = dBcontext;
            _logger = logger;
            //_context = context;
            //_userid = _context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
        }

        public async Task<T> AddAsync(T entity)
        {
            //entity.CreatedBy = _userid;
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            entity.isDeleted = true;
            //entity.DeletedBy = _userid;
            entity.DeletedAt = DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
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

        public async Task<T> GetById(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            //entity.UpdatedBy= _userid;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
