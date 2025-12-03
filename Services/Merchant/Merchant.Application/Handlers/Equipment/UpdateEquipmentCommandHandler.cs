using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Equipment;
using Merchants.Application.Exceptions;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Equipment
{
    public class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEquipmentRepository _ChannelRepository;

        public UpdateEquipmentCommandHandler(IEquipmentRepository merchantRepository, IMapper mapper,
            ILogger<UpdateEquipmentCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var equipment = await _ChannelRepository.GetById(request.Id);
                if (equipment == null)
                {
                    _logger.LogError($"Equipment not found");
                    return new Response
                    {
                        isSuccess = false,
                        ResponseCode = 0,
                        ResponseDescription = "Equipment Not Found",
                        Data = null
                    };
                }

                equipment.Name = request.Name;
                equipment.CategoryId = request.CategoryId;
                equipment.Status = "Active";
                equipment.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value ?? Guid.Empty.ToString());

                var result = await _ChannelRepository.UpdateAsync(equipment);
                _logger.LogInformation($"Equipment {equipment.Id} Updated Successfully.");

                return new Response
                {
                    isSuccess = true,
                    ResponseCode = 1,
                    ResponseDescription = "Equipment Updated",
                    Data = equipment
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Equipment update failed {ex.Message}.");
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
