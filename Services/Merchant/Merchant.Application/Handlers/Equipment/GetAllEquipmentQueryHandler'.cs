using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Equipment
{
    public class GetAllEquipmentQueryHandler : IRequestHandler<GetAllEquipmentQuery, Response>
    {
        private readonly IEquipmentRepository _repo;
        private readonly IComplaintCategory _categoryRepo;
        private readonly ILogger<GetAllEquipmentQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetAllEquipmentQueryHandler(IEquipmentRepository repo, ILogger<GetAllEquipmentQueryHandler> logger, IComplaintCategory categoryRepo, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetAllEquipmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var equipment = await _repo.GetAllAsync(x => x.isDeleted != true);
                var categories = await _categoryRepo.GetAllAsync();

                if (equipment.Count == 0)
                {
                    _logger.LogInformation($"No Equipment Found");
                    return new Response
                    {
                        isSuccess = false,
                        ResponseCode = 0,
                        ResponseDescription = "Equipment Not Found",
                        Data = null
                    };
                }
                else
                {
                    List<EquipmentResponse> equipmentList = new List<EquipmentResponse>();

                    foreach (var item in equipment)
                    {
                        EquipmentResponse equipmentItem = new();
                        equipmentItem = _mapper.Map<EquipmentResponse>(item);
                        equipmentItem.CategoryName = categories.FirstOrDefault(x => x.ID == equipmentItem.CategoryId)?.Category ?? "N/A";

                        equipmentList.Add(equipmentItem);
                    }

                    _logger.LogInformation($"Get All Equipment Successful.");
                    return new Response
                    {
                        isSuccess = true,
                        ResponseCode = 1,
                        ResponseDescription = "Equipments Found Successfully",
                        Data = equipmentList
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllChannel addition failed {ex.Message}.");
                return new Response
                {
                    isSuccess = false,
                    ResponseCode = 0,
                    ResponseDescription = ex.Message,
                    Data = null
                };
            }
        }
    }
}
