using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Exceptions;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ManagementHierarchy
{
    //internal class UpdateManagementHierarchyCommandHandler
    //{
    //}

    public class UpdateManagementHierarchyCommandHandler : IRequestHandler<UpdateManagementHierarchyCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IManagementHierarchy _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public UpdateManagementHierarchyCommandHandler(IManagementHierarchy merchantRepository, IMapper mapper,
            ILogger<UpdateManagementHierarchyCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ComplaintCategoryRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }

        public async Task<Response> Handle(UpdateManagementHierarchyCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var checkmerchant = await _ComplaintCategoryRepository.AddComplaintCategoryAsync(request.);
                //if (checkmerchant != null)
                //{
                //    response.isSuccess = false;
                //    response.ResponseCode = 0;
                //    response.ResponseDescription = "Merchant Code Already Exists.";
                //    response.Data = null;
                //    return response;
                //}
                //var merchantEntity = _mapper.Map<ComplaintCategory>(request);
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ManagementHierarchy>(request);
                var MerchantToUpdate = await _ComplaintCategoryRepository.GetById(request.ID);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"ManagementHierarchy Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.ID);
                }

                MerchantToUpdate.Name = request.Name ?? MerchantToUpdate.Name;
                MerchantToUpdate.POCName = request.POCName ?? MerchantToUpdate.POCName;
                MerchantToUpdate.POCEmail = request.POCEmail ?? MerchantToUpdate.Status;
                MerchantToUpdate.Address = request.Address ?? MerchantToUpdate.Address;
                MerchantToUpdate.POCNumber = request.POCNumber ?? MerchantToUpdate.POCNumber;
                MerchantToUpdate.OtherContact = request.OtherContact ?? MerchantToUpdate.OtherContact;
                MerchantToUpdate.OtherEmail = request.OtherEmail ?? MerchantToUpdate.OtherEmail;
                MerchantToUpdate.ParentID = request.ParentID ?? MerchantToUpdate.ParentID;
                MerchantToUpdate.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);



                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ManagementHierarchy>(MerchantToUpdate);
                //merchantEntity.Posted = 0;
                //merchantEntity.ReasonCode = "Created";
                //merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
                //merchantEntity.CreatedBy = new Guid();
                //merchantEntity.CreatedBy = new Guid("00000000-0000-0000-0000-000000000001");  // Static GUID equivalent of '1'
                //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
                //merchantEntity.Status = "1";

                var generatedMerchant = await _ComplaintCategoryRepository.UpdateAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "ManagementHierarchy updated Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"ManagementHierarchy {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"ManagementHierarchy addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
