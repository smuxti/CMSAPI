using AutoMapper;
using MediatR;
using Merchants.Application.Behaviours;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Core.OneLink;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Merchants.Application.Handlers.Merchants
{
    internal class MerchantPostCommandHandler : IRequestHandler<MerchantPostCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepository;
        public MerchantPostCommandHandler(IMerchantRepository merchantRepository, IMapper mapper, ILogger<MerchantPostCommandHandler> logger)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response> Handle(MerchantPostCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var merchant = await _merchantRepository.GetByMerchantCode(request.MerchantCode);
                if (merchant == null)
                {
                    _logger.LogError($"Merchant {request.MerchantCode} not found.");
                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = $"Merchant {request.MerchantCode} not found.";
                    return response;
                }

                string url = string.Empty;

                if (string.IsNullOrEmpty(request.ReasonCode))
                {
                    var mapped1link = _mapper.Map<CreateMerchant>(merchant);
                    mapped1link.merchantDetails.contactDetails.dept = "Main";
                    mapped1link.merchantDetails.postalAddress.subDept = "0001";

                    url = "/1Link/createMerchantProfile";

                    var postedmerchant = await _merchantRepository.PostMerchantOneLink(mapped1link, url);
                    if (postedmerchant == null)
                    {
                        _logger.LogError($"Merchant {request.MerchantCode} failed to post.");
                        response.isSuccess = false;
                        response.ResponseCode = 0;
                        response.ResponseDescription = $"Merchant {request.MerchantCode} failed to post.";
                        return response;
                    }


                    merchant.Posted = 1;
                    await _merchantRepository.UpdateAsync(merchant);
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = $"Merchant {request.MerchantCode} posted.";
                    response.Data = postedmerchant;
                    return response;
                }
                else
                {
                    var mapped1link = _mapper.Map<UpdateMerchant>(merchant);
                    mapped1link.merchantDetails.contactDetails.dept = "Main";
                    mapped1link.merchantDetails.postalAddress.subDept = "0001";
                    mapped1link.merchantDetails.reasonCode = request.ReasonCode;
                    url = "/1Link/updateMerchantProfile";
                    string reasonDescription = ReasonCodeHelper.GetReasonDescription(request.ReasonCode);
                    if (reasonDescription == "Invalid Reason Code")
                    {
                        _logger.LogError($"Merchant {request.MerchantCode} failed to post. || : {reasonDescription} : || Code: {request.ReasonCode}");
                        response.isSuccess = false;
                        response.ResponseCode = 0;
                        response.ResponseDescription = $"Merchant {request.MerchantCode} failed to post : {reasonDescription} : Code: {request.ReasonCode}";
                        return response;
                    }
                    mapped1link.merchantDetails.merchantStatus =
                     (merchant.isDeleted == true && merchant.Status == "InActive") ? "02" :
                     (merchant.Status == "Active" && merchant.isDeleted == false) ? "00" :
                     (merchant.Status == "InActive" && merchant.isDeleted == false) ? "01" : "-1";

                    _logger.LogInformation($"Merchant {request.MerchantCode}: {reasonDescription} : Code: {request.ReasonCode}");

                    var postedmerchant = await _merchantRepository.PostMerchantOneLink(mapped1link, url);
                    if (postedmerchant == null)
                    {
                        _logger.LogError($"Merchant {request.MerchantCode} failed to post.");
                        response.isSuccess = false;
                        response.ResponseCode = 0;
                        response.ResponseDescription = $"Merchant {request.MerchantCode} failed to post.";
                        return response;
                    }


                    merchant.ReasonCode = request.ReasonCode;
                    await _merchantRepository.UpdateAsync(merchant);
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = $"Merchant {request.MerchantCode} posted.";
                    response.Data = postedmerchant;
                    return response;
                }


                //if (merchant.Posted.Equals(0))
                //    url = "/1Link/createMerchantProfile";
                //else
                //    url = "/1Link/updateMerchantProfile";


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
