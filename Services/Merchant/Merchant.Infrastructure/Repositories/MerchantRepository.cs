using AutoMapper;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
//using Merchants.Core.OneLink;
using Merchants.Infrastructure.Data;
//using Merchants.Infrastructure.OneLink;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static StackExchange.Redis.Role;

namespace Merchants.Infrastructure.Repositories
{
    internal class MerchantRepository : AsyncRepository<Merchant>, IMerchantRepository
    {
        //private readonly OneLinkService _oneLink;
        private readonly IMapper _mapper;
        //public MerchantRepository(MerchantContext merchantContext, ILogger<MerchantRepository> logger, OneLinkService oneLink, IMapper mapper) : base(merchantContext, logger)
        //{
        //    _oneLink = oneLink;
        //    _mapper = mapper;
        //}

        public async Task<IEnumerable<Merchant>> GetMerchantByName(string name)
        {
            _logger.LogInformation($"Get Merchant All");
           // return await _dbContext.Merchant.Where(m=> m.Name.Contains(name)).ToListAsync();
            return await _dbContext.Merchant.Where(x=>x.isDeleted==false).ToListAsync();
        }
        public async Task<Merchant> GetMerchantById(Guid Id)
        {
            _logger.LogInformation($"Get Merchant By Id {Id}");
            // return await _dbContext.Merchant.Where(m=> m.Name.Contains(name)).ToListAsync();
            return await _dbContext.Merchant.Where(x=>x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<Merchant> GetMerchantByEmail(string Email)
        {
            _logger.LogInformation($"Get Merchant By Email {Email}");
            // return await _dbContext.Merchant.Where(m=> m.Name.Contains(name)).ToListAsync();
            return await _dbContext.Merchant.Where(x => x.Email == Email).FirstOrDefaultAsync();
        }

        public async Task<Merchant> GetByMerchantCode(string merchantCode)
        {
            return await _dbContext.Merchant.Where(m => m.MerchantCode.Equals(merchantCode)).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            return await _dbContext.Banks.ToListAsync();
        }
        public async Task<IEnumerable<MerchantCategory>> GetAllMerchantCategories()
        {
            return await _dbContext.MerchantCategories.ToListAsync();
        }
        public async Task<IEnumerable<FeeSlab>> GetAllSlabs()
        {
            return await _dbContext.FeeSlabs.ToListAsync();
        }
        public async Task<Bank> GetBankById(int id)
        {
            return await _dbContext.Banks.Where(x=>x.Id==id).FirstOrDefaultAsync();
        }
        public async Task<CreateMerchantOneLinkResponse> PostMerchantOneLink(CreateMerchant createMerchant, string url)
        {
            _logger.LogInformation($"Post Merchant Onelink {createMerchant.merchantDetails.merchantName}.");
            string token = await _oneLink.Get1LinkToken();
            if (token == null)
            {
                _logger.LogError($"Failed to generated token against merchant {createMerchant.merchantDetails.merchantName}");
                return null;
            }
            if (!string.IsNullOrEmpty(token))
                _logger.LogInformation($"token generated against merchant {createMerchant.merchantDetails.merchantName}");

            CreateMerchantOneLinkResponse createdmerchant = await _oneLink.PostMerchant(createMerchant, token, url);
            if (createdmerchant == null)
            {
                _logger.LogError($"Failed to post merchant against {createMerchant.merchantDetails.merchantName}");
                return null;
            }
            return createdmerchant;
        }
        public async Task<CreateMerchantOneLinkResponse> PostMerchantOneLink(UpdateMerchant createMerchant, string url)
        {
            _logger.LogInformation($"Update Merchant Onelink {createMerchant.merchantDetails.merchantName}.");
            string token = await _oneLink.Get1LinkToken();
            if (token == null)
            {
                _logger.LogError($"Failed to generated token against merchant {createMerchant.merchantDetails.merchantName}");
                return null;
            }
            if (!string.IsNullOrEmpty(token))
                _logger.LogInformation($"token generated against merchant {createMerchant.merchantDetails.merchantName}");

            CreateMerchantOneLinkResponse createdmerchant = await _oneLink.PostMerchant(createMerchant, token, url);
            if (createdmerchant == null)
            {
                _logger.LogError($"Failed to update merchant against {createMerchant.merchantDetails.merchantName}");
                return null;
            }
            return createdmerchant;
        }


    }
}
