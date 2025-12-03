using Merchants.Core.Entities;
using Merchants.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Infrastructure.Data;
using System.Numerics;


namespace Merchants.Core.Interfaces
{
    public interface IEscalation:IAsyncRepository<Escalation>  
    {
        Task<IEnumerable<Escalation>> GetEscalationAsyn();
        Task<Escalation> AddEscalationAsync(Escalation escalation);
        Task<IEnumerable<Escalation>> AddEsalationsAs(IEnumerable<Escalation> escalations);

        Task<Escalation> GetEscalationByID(int ID);
        Task<IList<EscalationView>> GetEscalationByCategory(int CategoryID, int CoomplaintTypeID);
        Task<IEnumerable<Escalation>> GetEscalationByManagementID(int ID);
        Task<IList<EscalationView>> GetEscalationByMerchant(int LevelID);
    }
}
