using Merchants.Core.Entities;

namespace Merchants.Core.Interfaces
{
    public interface ITerminalRepository: IAsyncRepository<Terminal>
    {
        Task<Terminal> GetTerminalBySerial(string serial);
        Task<List<Terminal>> GetTerminalByMerchantID(Guid merchantID);
        Task<Terminal> GetTerminalByCode(string code);
        Task<IEnumerable<Tenant>> GetAllTenants();
        Task<Terminal> GetTerminalByCodeAndMerchantId(string code, Guid merchantId);
    }
}
