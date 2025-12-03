using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Commands.Complaint;
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

namespace Merchants.Application.Handlers.Complaint
{
   
    //public class UpdateComplaintCommandHandler : IRequestHandler<UpdateCompaintCommand, Response>
    //{
    //    private readonly ILogger _logger;
    //    private readonly IHttpContextAccessor _httpContextAccessor;
    //    private readonly IMapper _mapper;
    //    private readonly IComplaint _ChannelRepository;
    //    //private readonly Mail _mail;
    //    //private readonly IConfiguration _configuration;
    //    //private readonly string _baseUrl;
    //    //private readonly IRedisCacheService _redisCacheService;


    //    public UpdateComplaintCommandHandler(IComplaint merchantRepository, IMapper mapper,
    //        ILogger<UpdateComplaintCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
    //    {
    //        _ChannelRepository = merchantRepository;
    //        _mapper = mapper;
    //        _logger = logger;
    //        _httpContextAccessor = httpContextAccessor;
    //        //_mail = mail;
    //        //_configuration = configuration;
    //        ////_baseUrl = _configuration["Urls:ActivationUrl"];
    //        //_redisCacheService = redisCacheService;
    //    }

    //    public async Task<Response> Handle(UpdateCompaintCommand request, CancellationToken cancellationToken)
    //    {
    //        Response response = new Response();
    //        try
    //        {
    //            //var checkmerchant = await _ComplaintCategoryRepository.AddComplaintCategoryAsync(request.);
    //            //if (checkmerchant != null)
    //            //{
    //            //    response.isSuccess = false;
    //            //    response.ResponseCode = 0;
    //            //    response.ResponseDescription = "Merchant Code Already Exists.";
    //            //    response.Data = null;
    //            //    return response;
    //            //}
    //            //var merchantEntity = _mapper.Map<ComplaintCategory>(request);
    //            var CurentComplaint = await _ChannelRepository.GetById(request.Id);
    //            //CurentComplaint.Description = request.Description;              
    //            var merchantEntity = _mapper.Map<Merchants.Core.Entities.Complaint>(CurentComplaint);

    //            //merchantEntity.Posted = 0;
    //            //merchantEntity.ReasonCode = "Created";
    //            //merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
    //            //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
    //            //merchantEntity.CreatedBy = new Guid();
    //            //merchantEntity.CreatedBy = new Guid("00000000-0000-0000-0000-000000000001");  // Static GUID equivalent of '1'
    //            //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
    //            //merchantEntity.Status = "Active";

    //            var generatedMerchant = await _ChannelRepository.UpdateAsync(merchantEntity);

    //            response.isSuccess = true;
    //            response.ResponseCode = 1;
    //            response.ResponseDescription = "Channel updated Successfully.";
    //            response.Data = generatedMerchant;

    //            _logger.LogInformation($"Channel {merchantEntity} added successfully.");


    //            return response;

    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError($"Channel addition failed {ex.Message}.");
    //            response.isSuccess = false;
    //            response.ResponseCode = 0;
    //            response.ResponseDescription = ex.Message;
    //            response.Data = null;
    //            return response;
    //        }
    //    }
    //}



    public class UpdateComplaintCommandHandler : IRequestHandler<UpdateCompaintCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaint _merchantRepository;
        //private readonly IRedisCacheService _redisCacheService;

        public UpdateComplaintCommandHandler(IComplaint merchantRepository, IMapper mapper,
            ILogger<UpdateComplaintCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            //_httpContextAccessor = httpContextAccessor;
            //_redisCacheService = redisCacheService;
        }
        public async Task<Response> Handle(UpdateCompaintCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var MerchantToUpdate = await _merchantRepository.GetById(request.Id);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Merchant Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.Id);
                }
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintType>(MerchantToUpdate);

                //MerchantToUpdate.ComplainerID = request.ComplainerID ?? MerchantToUpdate.ComplainerID;
                //MerchantToUpdate. = request.ComplaintDetail ?? MerchantToUpdate.ComplaintDetail;
                MerchantToUpdate.Description = request.Description ?? MerchantToUpdate.Description;
                MerchantToUpdate.TypeID = request.TypeID;
                MerchantToUpdate.Remarks = request.Remarks ?? MerchantToUpdate.Remarks;
                MerchantToUpdate.CategoryID = request.CategoryID;
                MerchantToUpdate.MerchantID = request.MerchantID;
                MerchantToUpdate.ChannelID = request.ChannelID;
                MerchantToUpdate.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);

                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintType>(MerchantToUpdate);

                //_mapper.Map(request, MerchantToUpdate, typeof(UpdateCompaintCommand), typeof(IComplaint));
                //MerchantToUpdate.UpdatedBy = Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value, out var parsedGuid)
                //    ? parsedGuid
                //    : (Guid?)null;
                var UpdatedMerchant = await _merchantRepository.UpdateAsync(MerchantToUpdate);
                _logger.LogInformation($"Complaint {MerchantToUpdate} updated successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Update Successfully.";
                response.Data = request;

                //var account = await _redisCacheService?.GetCacheValueAsynca($"Id:{UpdatedMerchant.Id}");
                //if (account is not null)
                //{
                //    var deleteKey = await _redisCacheService.DeleteCacheValueAsync($"Id:{UpdatedMerchant.Id}");
                //}

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint updation failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Complaint Update Failed.";
                response.Data = request;
                return response;
            }

        }
    }
}
