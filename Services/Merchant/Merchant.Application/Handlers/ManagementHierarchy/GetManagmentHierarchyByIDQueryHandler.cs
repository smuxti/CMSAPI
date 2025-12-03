using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Queries;
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
    public class GetManagmentHierarchyByIDQueryHandler : IRequestHandler<ManagementHierarcyByIDQuery, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IManagementHierarchy _ManagementHierarchyRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public GetManagmentHierarchyByIDQueryHandler(IManagementHierarchy merchantRepository, IMapper mapper,
            ILogger<GetManagmentHierarchyByIDQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<Response> Handle(ManagementHierarcyByIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);


                //var complaint = await _escalationRepository.GetAllAsync(x => x.CategoryID == request.CategoryID && x.Type == request.Type);
                var escalation = await _ManagementHierarchyRepository.GetManagementHierarchyByID(request.ID);

                //var complaint = await _terminalRepository.GetAllAsync();


                if (escalation == null)
                {

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = null;

                    _logger.LogInformation($"Managment {escalation} not Found.");
                }
                else
                {
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " GetManagmentByID Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = escalation;

                    _logger.LogInformation($"GetManagmentByID {escalation}  successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetManagmentByID get failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }

    }
}
