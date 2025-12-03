using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Merchants.Infrastructure.Repositories
{
    internal class UserRepository : AsyncRepository<User>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(MerchantContext context, ILogger<UserRepository> logger, IConfiguration configuration) : base(context, logger)
        {
            _configuration = configuration;
        }
        //public async Task<User> GetUserByEmail(string email)
        //{
        //    _logger.LogInformation($"Get User By Name {email}.");
        //    return await _dbContext.Users.Where(m => m.Email.Contains(email) && m.isDeleted != true).FirstOrDefaultAsync();
        //}
        public async Task<User> GetUserById(Guid Id)
        {
            _logger.LogInformation($"Get User By Id {Id}.");
            return await _dbContext.Users.Where(m => m.Id == Id && m.isDeleted != true).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByUserame(string name)
        {
            _logger.LogInformation($"Get User By Name {name}.");
            var a = await _dbContext.Users.Where(m => /*m.Email.Equals(name) ||*/ m.Username.Equals(name) && m.isDeleted != true).FirstOrDefaultAsync();
            return a;
        }
        public async Task<UserType> GetRoleByTypeId(int Id)
        {
            _logger.LogInformation($"Get Role By Id {Id}.");
            return await _dbContext.UserTypes.Where(m => m.TypeCode.Equals(Id)).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<int>> GetRoutesByRoleTypeId(int Id)
        {
            _logger.LogInformation($"Get Role By Id {Id}.");
            var routs = await _dbContext.RoleRoutes.Where(m => m.RoleTypeId.Equals(Id)).Select(m => m.RouteId).ToListAsync();

            return routs;
        }

        public async Task<IEnumerable<UserType>> GetUserTypes()
        {
            _logger.LogInformation($"Get UserTypes");
            var routs = await _dbContext.UserTypes.ToListAsync();

            return routs;
        }
        public async Task<IEnumerable<User>> GetAllUserByMerchantId(int Id)
        {
            _logger.LogInformation($"Get User By MerchantId");
            var users = await _dbContext.Users.Where(x => x.MerchantId == Id && x.isDeleted != true).ToListAsync();
            return users;
        }

        public async Task<User> GetUserByRefreshToken(string token)
        {
            var refreshToke = await _dbContext.Users.Where(x=>x.RefreshToken == token).FirstOrDefaultAsync();
            return refreshToke;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }

        #region Routes
        public async Task<Routes> AddRoutes(Routes model)
        {
            var a = await _dbContext.Routes.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
        public async Task DeleteRoutes(Routes model)
        {
            _dbContext.Routes.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<Routes> UpdateRoutes(Routes model)
        {
            _dbContext.Entry(model).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return model;
        }
        public async Task<Routes> GetRoutesById(int Id)
        {
            return await _dbContext.Set<Routes>().FindAsync(Id);
        }
        public async Task<IReadOnlyList<Routes>> GetAllRoutes()
        {
            return await _dbContext.Set<Routes>().ToListAsync();
        }

        public async Task<RoleRouts> AddRoleRoutes(RoleRouts model)
        {
            await _dbContext.RoleRoutes.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
        public async Task DeleteRoleRoutes(RoleRouts model)
        {
            _dbContext.RoleRoutes.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<RoleRouts> GetRoleRoutesByTypeIdAndRouteId(int TypeId, int RoutePathId)
        {
            return await _dbContext.RoleRoutes.Where(x => x.RoleTypeId == TypeId && x.RoutePathId == RoutePathId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<RoleRouts>> GetRoleRoutesByTypeId(int TypeId)
        {
            return await _dbContext.RoleRoutes.Where(x => x.RoleTypeId == TypeId).ToListAsync();
        }
        #endregion Routes

    }
}
