using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Commands.ManagementHierarchy;
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

namespace Merchants.Application.Handlers.ManagementHierarchy
{
    //internal class AddManagementHierarchyCommandHandler
    //{
    //}

    public class AddManagementHierarchyCommandHandler : IRequestHandler<AddManagementHierarchyCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IManagementHierarchy _ManagementHierarchyRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddManagementHierarchyCommandHandler(IManagementHierarchy merchantRepository, IMapper mapper,
            ILogger<AddManagementHierarchyCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ManagementHierarchyRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }

        public async Task<Response> Handle(AddManagementHierarchyCommand request, CancellationToken cancellationToken)
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
                //var merc = await _ComplaintCategoryRepository.GetById(request.id);
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ManagementHierarchy>(request);

                if(merchantEntity.POCEmail == null || merchantEntity.POCEmail == "string") 
                {
                    _logger.LogInformation($"POC Email is empty.");


                    response.Data = new
                    {
                        Message = "POC Email is empty.",

                    };
                    response.isSuccess = false;
                    response.ResponseCode = 1;
                    return response;
                }
                if (merchantEntity.Name == null || merchantEntity.Name == "string")
                {
                    _logger.LogInformation($"Hierarcy name is empty.");


                    response.Data = new
                    {
                        Message = "Hierarcy name is empty.",

                    };
                    response.isSuccess = false;
                    response.ResponseCode = 1;
                    return response;
                }
                if (merchantEntity.POCName == null || merchantEntity.POCName == "string")
                {
                    _logger.LogInformation($"POC name is empty.");


                    response.Data = new
                    {
                        Message = "POC name is empty.",

                    };
                    response.isSuccess = false;
                    response.ResponseCode = 1;
                    return response;
                }

                var parentId = (merchantEntity.ParentID == null  || merchantEntity.ParentID == 0) ? -1 : merchantEntity.ParentID;

                if (parentId > 0)
                {
                    var checkParent = await _ManagementHierarchyRepository.GetAllAsync(x => x.ID == merchantEntity.ParentID);
                    if (checkParent.Count == 0)
                    {
                        _logger.LogInformation($"Invalid Parent : {merchantEntity.ParentID}.");


                        response.Data = new
                        {
                            Message = "Invalid Parent.",
                            
                        };
                        response.isSuccess = false;
                        response.ResponseCode = 1;
                        return response;
                    }
                }
                //else if(!(request.ManagementType > 0) && parentId < 0)
                //{
                //    var checkParent = await _ManagementHierarchyRepository.GetAllAsync(x => x.ParentID == null && x.ManagementType == 0);
                //    if (checkParent.Count != 0)
                //    {
                //        var data = checkParent.FirstOrDefault();
                //        _logger.LogInformation($"Parent record already exists : {data.Name}.");


                //        response.Data = new
                //        {
                //            Message = "Parent record already exists : " + data.Name + ", " + data.POCName + "."

                //        };
                //        response.isSuccess = false;
                //        response.ResponseCode = 1;
                //        return response;
                //    }
                //}

                merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                merchantEntity.Status = "1";

                var Escalation = "";
                int EscalationId;
                var existingComplainer = await _ManagementHierarchyRepository.GetAllAsync(x =>  x.POCName == merchantEntity.POCName && x.isDeleted == false);

                //var deletedComplainer = existingComplainer.FirstOrDefault(x => x.isDeleted == true);

                if (existingComplainer.Any() )
                {
                    var firstComplainer = existingComplainer.First();
                    Escalation = firstComplainer.POCEmail ?? string.Empty;
                    EscalationId = firstComplainer.ID;
                    _logger.LogInformation($"Management Hierarchy already exists with ID: {EscalationId}. Using existing Management Hierarchy.");


                    response.Data = new
                    {
                        Message = "Management Hierarchy already exists.",
                        EscalationDetails = Escalation
                    };
                    response.isSuccess = false;
                    response.ResponseCode = 1;
                }
                else
                {
                    response.ResponseDescription = "Management Hierarchy Created Successfully.";
                    var generatedMerchant = await _ManagementHierarchyRepository.AddAsync(merchantEntity);
                    EscalationId = generatedMerchant.ID;
                    Escalation = generatedMerchant.POCEmail ?? string.Empty;
                    response.Data = generatedMerchant;
                    _logger.LogInformation($"New Management Hierarchy added with ID: {EscalationId}.");
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                }




                //var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);

                //response.isSuccess = true;
                //response.ResponseCode = 1;
                //response.ResponseDescription = "Management Hierarchy Created  Successfully.";
                //response.Data = generatedMerchant;

                //_logger.LogInformation($"Management Hierarchy  {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Management Hierarchy  addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
