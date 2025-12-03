using Merchants.Application.Commands;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers
{
    public class DeleteRoleRoutsCommandHandler : IRequestHandler<DeleteRoleRoutsCommand, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteRoleRoutsCommandHandler> _logger;

        public DeleteRoleRoutsCommandHandler(IUserRepository userRepository, ILogger<DeleteRoleRoutsCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(DeleteRoleRoutsCommand request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var route = await _userRepository.GetRoleRoutesByTypeIdAndRouteId(request.UserTypeCode, request.RoutePathId);
                await _userRepository.DeleteRoleRoutes(route);
                _logger.LogError($"Record Deleted");
                baseResponse.ResponseCode = 1;
                baseResponse.ResponseDescription = "Record Deleted";
                baseResponse.isSuccess = true;
                baseResponse.Data = route;
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
