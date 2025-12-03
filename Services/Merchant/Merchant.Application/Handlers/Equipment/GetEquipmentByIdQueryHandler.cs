using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Equipment
{
    public class GetEquipmentByIdQueryHandler : IRequestHandler<GetEquipmentById, Response>
    {
        private readonly IEquipmentRepository _repo;
        private readonly ILogger<GetEquipmentByIdQueryHandler> _logger;
        public GetEquipmentByIdQueryHandler(IEquipmentRepository repo, ILogger<GetEquipmentByIdQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Response> Handle(GetEquipmentById request, CancellationToken cancellationToken)
        {
            try
            {
                var equipment = await _repo.GetById(request.Id);

                if (equipment is not null)
                {
                    _logger.LogInformation($"Get Equipment By Id Successful.");
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
                    _logger.LogInformation($"Equipment with Id: {request.Id} Not Found.");
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
                _logger.LogError($"Get Equipment By Id Failed: {ex.Message}.");
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
