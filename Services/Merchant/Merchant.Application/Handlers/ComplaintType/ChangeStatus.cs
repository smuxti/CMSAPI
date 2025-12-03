using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Exceptions;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ComplaintType
{
    public class ChangeStatus : IRequestHandler<ChangeComplaintTypeStatus, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintType _ComplaintTypeRepository;

        public ChangeStatus(IComplaintType complaintTypeRepository, IMapper mapper,
            ILogger<ChangeComplaintTypeStatus> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ComplaintTypeRepository = complaintTypeRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }
        public async Task<Response> Handle(ChangeComplaintTypeStatus request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);


                var complaint = await _ComplaintTypeRepository.GetType(request.ID);

                //var complaint = await _terminalRepository.GetAllAsync();

                if (complaint == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"ComplaintType {complaint} not Found.");
                }
                else
                {
                    complaint.Status = request.Status;
                    complaint.UpdatedAt = DateTime.UtcNow;
                    var compType = await _ComplaintTypeRepository.UpdateAsync(complaint);

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " Status has been changed Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"ComplaintType {complaint}  successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"ComplaintType addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }

    
}
