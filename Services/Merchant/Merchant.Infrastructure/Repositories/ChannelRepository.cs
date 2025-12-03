using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Repositories
{
    public class ChannelRepository : AsyncRepository<Channel>, IChannel
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<ChannelRepository> _logger;

        public ChannelRepository(MerchantContext merchantContext, ILogger<ChannelRepository> logger)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }

        public Task<Channel> AddChannelAsync(Channel channel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Channel>> GetAllChannelAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Channel> GetCHANELByID(int ID)
        {
            var ManagementHierarchy = await _merchantContext.Channels.Where(x => x.ID == ID).FirstOrDefaultAsync();


            return ManagementHierarchy;
        }
    }
}
