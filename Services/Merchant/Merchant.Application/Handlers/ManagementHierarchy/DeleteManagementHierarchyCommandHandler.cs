using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Escalation;
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
    //internal class DeleteManagementHierarchyCommandHandler
    //{
    //}

    public class DeleteManagementHierarchyCommandHandler : IRequestHandler<DeleteManagementHierarchyCommand, Response>
    {


        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IManagementHierarchy _IChannelRepository;
        public DeleteManagementHierarchyCommandHandler(IManagementHierarchy merchantRepository, IMapper mapper, ILogger<DeleteEscalationCommand> logger, IHttpContextAccessor httpContextAccessor)
        {
            _IChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteManagementHierarchyCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var MerchantToBeDeleted = await _IChannelRepository.GetById(request.Id);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"ManagementHierarchy Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.Id);

                }
                MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _IChannelRepository.DeleteAsync(MerchantToBeDeleted);
                _logger.LogInformation($"ManagementHierarchy {MerchantToBeDeleted} deleted successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "ManagementHierarchy deleted Successfully.";
                //response.Data = generatedMerchant;

                _logger.LogInformation($"ManagementHierarchy {MerchantToBeDeleted} deleted successfully.");


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
