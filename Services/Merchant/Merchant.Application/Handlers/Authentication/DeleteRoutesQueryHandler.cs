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
    public class DeleteRoutesQueryHandler : IRequestHandler<DeleteRoutesQuery,Response>
    {
        private readonly ILogger<DeleteRoutesQueryHandler> _logger;
        private readonly IUserRepository _userRepository;

        public DeleteRoutesQueryHandler(ILogger<DeleteRoutesQueryHandler> logger , IUserRepository userRepository)
        {
            this._logger = logger;
            this._userRepository = userRepository;
        }

        public async Task<Response> Handle(DeleteRoutesQuery request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var model = await _userRepository.GetRoutesById(request.Id);
                if (model == null)
                {
                    baseResponse.isSuccess = false;
                    baseResponse.ResponseDescription = "No Route Found";
                    baseResponse.ResponseCode = 0;
                    baseResponse.Data = null;
                    return baseResponse;
                }
             

                await _userRepository.DeleteRoutes(model);
                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Route deleted";
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
