using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ComplaintCategory
{


    public class GetAllComplaintCategorybyQueryHandler : IRequestHandler<GetAllComplaintCategoryQuery ,Response>
        {
        private readonly IComplaintCategory _categoryRepository;
        private readonly ILogger<GetAllComplaintCategorybyQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IEquipmentRepository _equipmentRepository;

        public GetAllComplaintCategorybyQueryHandler(IComplaintCategory categoryRepository, IMapper mapper, ILogger<GetAllComplaintCategorybyQueryHandler> logger, IEquipmentRepository equipmentRepository)
        {
            _mapper = mapper;

            _categoryRepository = categoryRepository;
            _logger = logger;
            _equipmentRepository = equipmentRepository;
        }

        public async Task<Response> Handle(GetAllComplaintCategoryQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);

                var categories = await _categoryRepository.GetAllAsync(x => x.isDeleted != true);

                //var complaintCategoriestypes = await _terminalRepository.GetAllAsync();

                if (categories.Count == 0)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = categories;

                    _logger.LogInformation($"Category {categories} not Found.");
                }
                else
                {
                    var equipments = await _equipmentRepository.GetAllAsync();
                    List<CategoryDTO> categoryList = new();

                    foreach (var category in categories)
                    {
                        var categoryDTO = _mapper.Map<CategoryDTO>(category);
                        categoryDTO.HasEquipment = equipments?.Any(e => e.CategoryId == category.ID && e.isDeleted == false && e.Status != "InActive") ?? false;
                        categoryList.Add(categoryDTO);
                    }

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "GetAll Complaint Category Successfully.";
                    response.Data = categoryList;
                }
            _logger.LogInformation($"Category {categories}  successfully.");

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
