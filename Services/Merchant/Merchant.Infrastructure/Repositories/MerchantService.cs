using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Repositories
{
    public class MerchantService : AsyncRepository<Merchant>, IMerchant
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<MerchantService> _logger;

        public MerchantService(MerchantContext merchantContext, ILogger<MerchantService> logger)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }
        public Task<Merchant> AddMerchantAsync(Merchant merchant)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMerchant()
        {
            throw new NotImplementedException();
        }

        public async Task<Merchant> GetMerchantByID(int ID)
        {

            var merchant = await _merchantContext.Merchants.Where(x => x.ID == ID).FirstOrDefaultAsync();
            if (merchant == null)
            {
                // Return an empty Merchant object with default or empty string values
                return new Merchant
                {
                    // Assuming the Merchant class has these properties, set them to empty strings
                    MerchantName = string.Empty,
                

                    // Add other properties as necessary
                };
            }

            return merchant;
        }
        public async Task<Merchant> GetMerchantByZone(int ID)
        {

            var merchant = await _merchantContext.Merchants.Where(x => x.Zone == ID).FirstOrDefaultAsync();

            return merchant;
        }
        public async Task<Merchant> GetMerchantByArea(int ID)
        {

            var merchant = await _merchantContext.Merchants.Where(x => x.Area == ID).FirstOrDefaultAsync();
            return merchant;
        }

        public async Task<string> GetMerchantCode()
        {
            try
            {
                //string temp = await _merchantContext.Merchants.MaxAsync(x => x.MerchantCode);
                //if (temp == null) temp = DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString() + "000001";
                //else temp = (int.Parse(temp) + 1).ToString();
                //return temp;

                string temp = await _merchantContext.Merchants.MaxAsync(x => x.MerchantCode);

                if (string.IsNullOrEmpty(temp))
                {
                    temp = "00000001";
                }
                else
                {
                    long currentValue = Convert.ToInt64(temp, 16);
                    currentValue++;
                    temp = currentValue.ToString("X8"); //8-digit
                }

                return temp;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMerchant(int Id)
        {
            bool resp;
            var result = _merchantContext.Merchants.Where(x => x.ID == Id).FirstOrDefault();
            if (result != null)
            {
                await _merchantContext.Merchants.ExecuteDeleteAsync();
                await _merchantContext.SaveChangesAsync();
                resp = true;
            }
            else
            {
                resp = false;
            }
            return resp;
        }

        public Task<IEnumerable<Merchant>> GetMerchantsAsyn()
        {
            throw new NotImplementedException();
        }
    }
}
