using Authentication.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Handlers
{
    public class GetRoutesByUserTypeQueryHandler : IRequestHandler<GetRoutesByUserTypeQuery, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetRoutesByUserTypeQueryHandler> _logger;

        public GetRoutesByUserTypeQueryHandler(IUserRepository userRepository, ILogger<GetRoutesByUserTypeQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(GetRoutesByUserTypeQuery request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var routes = await _userRepository.GetRoleRoutesByTypeId(request.TypeId);

                _logger.LogError($"Record found");
                baseResponse.ResponseCode = 1;
                baseResponse.ResponseDescription = "Record found";
                baseResponse.isSuccess = true;
                baseResponse.Data = routes;
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
