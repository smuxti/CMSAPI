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
    public class GetAllRoutesCommandQueryHandler : IRequestHandler<GetAllRoutesCommandQuery, Response>
    {
        private readonly ILogger<GetAllRoutesCommandQueryHandler> _logger;
        private readonly IUserRepository _repository;

        public GetAllRoutesCommandQueryHandler(ILogger<GetAllRoutesCommandQueryHandler> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response> Handle(GetAllRoutesCommandQuery request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var list = await _repository.GetAllRoutes();
                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Routes Fetched";
                baseResponse.ResponseCode = 1;
                baseResponse.Data = list;

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
