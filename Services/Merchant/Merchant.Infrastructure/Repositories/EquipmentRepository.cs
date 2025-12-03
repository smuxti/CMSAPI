using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Merchants.Infrastructure.Repositories
{
    public class EquipmentRepository : AsyncRepository<Equipment>, IEquipmentRepository
    {
        private readonly ILogger<EquipmentRepository> _logger;
        public EquipmentRepository(MerchantContext context, ILogger<EquipmentRepository> logger) : base(context, logger) 
        {
            _logger = logger; 
        }

        public async Task<IEnumerable<Equipment>> GetByCategoryId(int CategoryId)
        {
            return await _dbContext.Equipments.Where(x => x.CategoryId == CategoryId && !x.isDeleted).ToListAsync();
        }
    }
}
