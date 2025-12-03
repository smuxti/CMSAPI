using Authentication.Application.Commands;
using Authentication.Core.Interfaces;
using MediatR;
using Merchants.Application.Commands;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Handlers
{
    public class AddRoleRoutesCommandHandler : IRequestHandler<AddRoleRoutesCommand, Response>
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddRoleRoutesCommandHandler> _logger;

        public AddRoleRoutesCommandHandler(IUserTypeRepository userTypeRepository, IUserRepository userRepository, ILogger<AddRoleRoutesCommandHandler> logger)
        {
            _userTypeRepository = userTypeRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(AddRoleRoutesCommand request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var route = await _userRepository.GetRoutesById(request.RoutePathId);
                var type = await _userTypeRepository.GetByUserTypeCode(request.UserTypeCode);
                if (route == null || type == null)
                {
                    _logger.LogError($"Data Not Available");
                    baseResponse.ResponseCode = 0;
                    baseResponse.ResponseDescription = "Data Not Available";
                    baseResponse.isSuccess = false;
                    baseResponse.Data = null;
                    return baseResponse;
                }
                RoleRouts record = new RoleRouts();
                record.RoutePathId = request.RoutePathId;
                record.RoleTypeId = request.UserTypeCode;
                record.RouteId = request.RouteId;
                var a =await _userRepository.AddRoleRoutes(record);

                _logger.LogError($"Record Added");
                baseResponse.ResponseCode = 1;
                baseResponse.ResponseDescription = "Record Added";
                baseResponse.isSuccess = true;
                baseResponse.Data = a;
                return baseResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($" In The Catch Block {ex.Message}");
                baseResponse.ResponseCode = 0;
                baseResponse.ResponseDescription = ex.Message;
                baseResponse.isSuccess = false;
                baseResponse.Data = null;
                return baseResponse;
                throw;
            }
        }
    }
}
