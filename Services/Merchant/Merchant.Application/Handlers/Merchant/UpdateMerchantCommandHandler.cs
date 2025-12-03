using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Exceptions;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Merchant
{
    //internal class UpdateMerchantCommandHandler
    //{
    //}

    public class UpdateMerchantCommandHandler : IRequestHandler<UpdateMerchantCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMerchant _merchantRepository;
        private readonly IZones _merchantLocation;
        //private readonly IRedisCacheService _redisCacheService;

        public UpdateMerchantCommandHandler(IMerchant merchantRepository, IMapper mapper,
            ILogger<UpdateMerchantCommandHandler> logger, IHttpContextAccessor httpContextAccessor, IZones merchantLocation)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _merchantLocation = merchantLocation;
            _httpContextAccessor = httpContextAccessor;
            //_redisCacheService = redisCacheService;
        }
        public async Task<Response> Handle(UpdateMerchantCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var MerchantToUpdate = await _merchantRepository.GetById(request.ID);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Merchant Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.ID);
                }


                MerchantToUpdate.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);


                MerchantToUpdate.Zone = request.Zone;
                MerchantToUpdate.Area = request.Area;
                MerchantToUpdate.MerchantName = request.MerchantName ?? MerchantToUpdate.MerchantName;
                MerchantToUpdate.City = request.City;
                MerchantToUpdate.MerchantAddress = request.MerchantAddress;
                MerchantToUpdate.OtherEmail = request.OtherEmail;
                MerchantToUpdate.Email = request.Email;
                MerchantToUpdate.Number = request.Number;
                MerchantToUpdate.OtherNumber = request.OtherNumber;

                var UpdatedMerchant = await _merchantRepository.UpdateAsync(MerchantToUpdate);
                _logger.LogInformation($"Merchant {MerchantToUpdate} updated successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Merchant Update Successfully.";
                response.Data = request;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Merchant updation failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Merchant Update Failed.";
                response.Data = request;
                return response;
            }

        }
    }
}
