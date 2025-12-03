using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Equipment
{
    public class DeleteEquipmentQueryHandler : IRequestHandler<DeleteEquipmentById, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEquipmentRepository _repo;
        public DeleteEquipmentQueryHandler(IEquipmentRepository repo, IMapper mapper, ILogger<DeleteEquipmentQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteEquipmentById request, CancellationToken cancellationToken)
        {
            try
            {
                var equipment = await _repo.GetById(request.Id);
                if (equipment == null)
                {
                    _logger.LogError($"Equipment Not found for deletion.");
                    return new Response
                    {
                        isSuccess = false,
                        ResponseCode = 0,
                        ResponseDescription = "Equipment Not Found",
                        Data = null
                    };
                }
                equipment.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value ?? Guid.Empty.ToString());
                await _repo.DeleteAsync(equipment);
                _logger.LogInformation($"Equipment Id: {equipment.Id} deleted successfully.");

                return new Response
                {
                    isSuccess = true,
                    ResponseCode = 1,
                    ResponseDescription = "Equipment Deleted Successfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Equipment deletion failed {ex.Message}.");
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
