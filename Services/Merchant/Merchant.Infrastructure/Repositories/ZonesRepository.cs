using Merchants.Core.Common;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Merchants.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Repositories
{
    public class ZonesRepository : AsyncRepository<ManagementHierarchy>, IZones
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<ZonesRepository> _logger;

        public ZonesRepository(MerchantContext merchantContext, ILogger<ZonesRepository> logger)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }

        public async Task<ManagementHierarchy> AddZoneAsync(ManagementHierarchy merchantLocation)
        {
            var merchantlocation = await _merchantContext.ManagementHierarchies.AddAsync(merchantLocation);

            // Saving changes to the database
            await _merchantContext.SaveChangesAsync();

            // Returning the added escalations (with the updated state, such as Ids if auto-generated)
            return merchantlocation.Entity;
        }


        public async Task<bool> DeleteZone(int Id)
        {
            var result = false;
            var merchantlocation = _merchantContext.ManagementHierarchies.Where(s=> s.ID == Id).FirstOrDefault();
            if(merchantlocation != null)
            {
                _merchantContext.ManagementHierarchies.Remove(merchantlocation);
                var saveResult = await _merchantContext.SaveChangesAsync();
                if (saveResult > 0)
                {
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public async Task<IEnumerable<ZoneView>> GetZoneAsyn()
        {
            var merchantZones = await _merchantContext.ManagementHierarchies.Where(s => s.ParentID == 0 && s.ManagementType == 3 && s.isDeleted == false).Select(s=> new ZoneView { ID = s.ID,Location = s.Name, POCName = s.POCName,POCEmail = s.POCEmail,POCNumber = s.POCNumber }) .ToListAsync();
            return merchantZones;
        }

        public async Task<IEnumerable<AreaView>> GetAreaAsyn(int ZoneID)
        {
            var merchantlocation = await (from c in _merchantContext.ManagementHierarchies
                                          //join e in _merchantContext.ManagementHierarchies on c.ID equals e.ParentID
                                          where c.ParentID == ZoneID && c.ManagementType == 2 && c.isDeleted == false
                                          select new AreaView
                                          {
                                              ID = c.ID,
                                              ZoneID = c.ParentID.Value,
                                              //Zone = e.Name,
                                              Location = c.Name,
                                              POCName = c.POCName,
                                              POCEmail = c.POCEmail,
                                              POCNumber = c.POCNumber
                                          }).ToListAsync();


            return merchantlocation;
        }

        public async Task<ZoneView> GetZoneByID(int Id)
        {
            var merchantlocation = await (from c in _merchantContext.ManagementHierarchies
                                          where c.ID == Id && c.isDeleted == false
                                          select new ZoneView
                                          {
                                              ID = c.ID,
                                              Location = c.Name,
                                              POCName = c.POCName,
                                              POCEmail = c.POCEmail,
                                              POCNumber = c.POCNumber
                                          }).FirstOrDefaultAsync();


            return merchantlocation;
        }

        public async Task<ZoneView>GetZoneByName(string Name)
        {
            var merchantlocation = await (from c in _merchantContext.ManagementHierarchies
                                          where c.Name == Name && c.isDeleted == false
                                          select new ZoneView
                                          {
                                              ID = c.ID,
                                              Location = c.Name,
                                              POCName = c.POCName,
                                              POCEmail = c.POCEmail,
                                              POCNumber = c.POCNumber
                                          }).FirstOrDefaultAsync();


            return merchantlocation;
        }



    }
}
