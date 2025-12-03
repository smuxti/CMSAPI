using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IChannel:IAsyncRepository<Channel> 
    {
        Task<Channel> AddChannelAsync(Channel channel);
        Task<IEnumerable<Channel>> GetAllChannelAsync();

        Task<Channel> GetCHANELByID(int ID);
    }
}
