using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IUserRepository: IAsyncRepository<User>
    {
        Task<User> GetUserByUserame(string name);
        //Task<User> GetUserByEmail(string name);
        Task<UserType> GetRoleByTypeId(int Id );
        Task<IEnumerable<int>> GetRoutesByRoleTypeId(int Id);
        Task<IEnumerable<UserType>> GetUserTypes();
        Task<IEnumerable<User>> GetAllUserByMerchantId(int Id);
        Task<User> GetUserByRefreshToken(string token);
        Task<User> GetUserById(Guid Id);
        Task<User> GetUserByEmail(string email);


        #region Routes
        Task<Routes> AddRoutes(Routes model);
        Task DeleteRoutes(Routes model);
        Task<Routes> UpdateRoutes(Routes model);
        Task<Routes> GetRoutesById(int Id);
        Task<IReadOnlyList<Routes>> GetAllRoutes();
        Task<RoleRouts> AddRoleRoutes(RoleRouts model);
        Task DeleteRoleRoutes(RoleRouts model);
        Task<RoleRouts> GetRoleRoutesByTypeIdAndRouteId(int TypeId, int RoutePathId);
        Task<IEnumerable<RoleRouts>> GetRoleRoutesByTypeId(int TypeId);
        #endregion Routes
    }
}
