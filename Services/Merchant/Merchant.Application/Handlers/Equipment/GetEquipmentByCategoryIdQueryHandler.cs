using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Equipment
{
    public class GetEquipmentByCategoryIdQueryHandler : IRequestHandler<GetEquipmentByCategoryId, Response>
    {
        private readonly IEquipmentRepository _repo;
        private readonly ILogger<GetEquipmentByCategoryIdQueryHandler> _logger;
        public GetEquipmentByCategoryIdQueryHandler(IEquipmentRepository repo, ILogger<GetEquipmentByCategoryIdQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Response> Handle(GetEquipmentByCategoryId request, CancellationToken cancellationToken)
        {
            try
            {
                var equipment = await _repo.GetByCategoryId(request.CategoryId);

                if (equipment is not null)
                {
                    _logger.LogInformation($"Get Equipment By Category Successful.");
                    return new Response
                    {
                        isSuccess = true,
                        ResponseCode = 1,
                        ResponseDescription = "Equipment Found Succesfully ",
                        Data = equipment
                    };
                }
                else
                {
                    _logger.LogInformation($"Equipment with CategoryId: {request.CategoryId} Not Found.");
                    return new Response
                    {
                        isSuccess = false,
                        ResponseCode = 0,
                        ResponseDescription = "Equipment Not Found",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Equipment By CategoryId Failed: {ex.Message}.");
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
