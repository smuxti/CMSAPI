using Merchants.Application.Commands;
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

namespace Authentication.Application.Handlers
{
    public class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand, Response>
    {
        private readonly ILogger<UpdateRouteCommandHandler> _logger;
        private readonly IUserRepository _repository;

        public UpdateRouteCommandHandler(ILogger<UpdateRouteCommandHandler> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response> Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var model = await _repository.GetRoutesById(request.Id);
                if (model == null)
                {
                    baseResponse.isSuccess = false;
                    baseResponse.ResponseDescription = "No Route Found";
                    baseResponse.ResponseCode = 0;
                    baseResponse.Data = null;
                    return baseResponse;
                }

                model.RouteName = request.RouteName;
                model.ModuleName = request.ModuleName;
                model.RoutePath = request.RoutePath;

                await _repository.UpdateRoutes(model);
                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Route updated";
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
