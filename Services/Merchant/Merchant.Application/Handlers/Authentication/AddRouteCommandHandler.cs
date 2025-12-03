using Authentication.Application.Commands;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
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
    public class AddRouteCommandHandler : IRequestHandler<AddRouteCommand, Response>
    {
        private readonly ILogger<AddRouteCommandHandler> _logger;
        private readonly IUserRepository _repository;

        public AddRouteCommandHandler(ILogger<AddRouteCommandHandler> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response> Handle(AddRouteCommand request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                Routes model = new Routes();

                var routes = await _repository.GetAllRoutes();
                int routeId = routes.Select(x => x.RouteId).DefaultIfEmpty(0).Max();
                routeId++;

                model.RouteId = routeId;
                model.RouteName = request.RouteName;
                model.ModuleName = request.ModuleName;
                model.RoutePath = request.RoutePath;

                await _repository.AddRoutes(model);
                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Route Added";
                baseResponse.ResponseCode = 1;
                baseResponse.Data = model;
                return baseResponse;

            }
            catch (Exception ex)
            {
                baseResponse.isSuccess = false;
                baseResponse.ResponseDescription = ex.Message;
                baseResponse.ResponseCode = 0;
                baseResponse.Data = null;
                return baseResponse;
                throw;
            }
        }
    }
}
