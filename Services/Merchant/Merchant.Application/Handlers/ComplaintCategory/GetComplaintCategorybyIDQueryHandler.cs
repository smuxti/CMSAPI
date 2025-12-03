using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ComplaintCategory
{
    public class GetComplaintCategorybyIDQueryHandler : IRequestHandler<GetComplaintCategoryByIDQuery, Response>
    {
        private readonly IComplaintCategory _categorytRepository;
        private readonly ILogger<GetComplaintCategorybyIDQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IEquipmentRepository _equipmentRepository;

        public GetComplaintCategorybyIDQueryHandler(IComplaintCategory categoryRepository, IMapper mapper, ILogger<GetComplaintCategorybyIDQueryHandler> logger, IEquipmentRepository equipmentRepository)
        {
            _mapper = mapper;
            _categorytRepository = categoryRepository;
            _logger = logger;
            _equipmentRepository = equipmentRepository;
        }

        public async Task<Response> Handle(GetComplaintCategoryByIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var category = await _categorytRepository.GetComplaintCategoryByID(request.ID);

                if (category == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    response.Data = category;

                    _logger.LogInformation($"Category {category} not Found.");
                }
                else
                {
                    var equipments = await _equipmentRepository.GetAllAsync();

                    var categoryDTO = _mapper.Map<CategoryDTO>(category);
                    categoryDTO.HasEquipment = equipments?.Any(e => e.Id == category.ID) ?? false;

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "Get Complaint Category By ID Successfully.";
                    response.Data = categoryDTO;
                }
                _logger.LogInformation($"Category {category}  successfully.");

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Category By ID failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }
}
