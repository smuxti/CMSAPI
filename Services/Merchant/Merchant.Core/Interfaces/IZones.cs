using Merchants.Core.Common;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IZones : IAsyncRepository<ManagementHierarchy>
    {
        Task<IEnumerable<ZoneView>> GetZoneAsyn();
        Task<IEnumerable<AreaView>> GetAreaAsyn(int ZoneID);
        Task<ManagementHierarchy> AddZoneAsync(ManagementHierarchy merchantLocation);

        Task<ZoneView> GetZoneByID(int Id);
        Task<bool> DeleteZone(int Id);

        Task<ZoneView> GetZoneByName(string Name);
    }
}
