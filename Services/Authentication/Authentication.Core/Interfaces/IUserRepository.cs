using Authentication.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IUserRepository: IAsyncRepository<User>
    {
        Task<User> GetUserByUserame(string name);
        Task<User> GetUserByEmail(string name);
        Task<UserType> GetRoleByTypeId(int Id );
        Task<IEnumerable<int>> GetRoutesByRoleTypeId(int Id);
        Task<IEnumerable<UserType>> GetUserTypes();
        Task<IEnumerable<User>> GetAllUserByMerchantId(Guid Id);
        Task<User> GetUserByRefreshToken(string token);
    }
}
