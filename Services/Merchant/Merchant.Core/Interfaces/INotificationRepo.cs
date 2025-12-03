using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface INotificationRepo
    {
        Task<string> NotificationToManagement(string managementid, string message);
    }
}
