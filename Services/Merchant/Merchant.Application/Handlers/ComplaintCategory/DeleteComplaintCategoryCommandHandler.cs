using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
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

namespace Merchants.Application.Handlers.ComplaintCategory
{
    public class DeleteComplaintCategoryCommandHandler : IRequestHandler<DeleteComplaintCategoryCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintCategory _ComplaintCategoryRepository;
        public DeleteComplaintCategoryCommandHandler(IComplaintCategory merchantRepository, IMapper mapper, ILogger<DeleteComplaintCategoryCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ComplaintCategoryRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteComplaintCategoryCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var MerchantToBeDeleted = await _ComplaintCategoryRepository.GetById(request.Id);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"Category Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.Id);

                }
                MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _ComplaintCategoryRepository.DeleteAsync(MerchantToBeDeleted);
                _logger.LogInformation($"Complaint Category {MerchantToBeDeleted} deleted successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Category deleted Successfully.";
                //response.Data = generatedMerchant;

                _logger.LogInformation($"Category Category {MerchantToBeDeleted} deleted successfully.");


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Category addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }

    }
}
