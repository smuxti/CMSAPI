using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Equipment;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Equipment
{
    public class AddEquipmentCommandHandler : IRequestHandler<AddEquipmentCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEquipmentRepository _repo;

        public AddEquipmentCommandHandler(IEquipmentRepository repo, IMapper mapper,
            ILogger<AddEquipmentCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Handle(AddEquipmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var equipment = _mapper.Map<Merchants.Core.Entities.Equipment>(request);

                equipment.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value ?? Guid.Empty.ToString()); //new Guid("00000000-0000-0000-0000-000000000001");  // Static GUID equivalent of '1'
                equipment.Status = "Active";

                var result = await _repo.AddAsync(equipment);

                if (result is not null)
                    return new Response
                    {
                        isSuccess = true,
                        ResponseCode = 1,
                        ResponseDescription = "Equipment Created Successfully",
                        Data = result
                    };
                else
                    return new Response
                    {
                        isSuccess = false,
                        ResponseCode = 0,
                        ResponseDescription = "Equipment Creation Failed",
                        Data = null
                    };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Channel addition failed {ex.Message}.");
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
