using Authentication.Application.Queries;
using Merchants.Application.Responses;
using Authentication.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Handlers
{
    public class GetAllUserTypeQueryHandler : IRequestHandler<GetAllUserTypeQuery, Response>
    {
        private readonly ILogger<GetAllUserTypeQueryHandler> _logger;
        private readonly IUserTypeRepository _repository;

        public GetAllUserTypeQueryHandler(ILogger<GetAllUserTypeQueryHandler> logger, IUserTypeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response> Handle(GetAllUserTypeQuery request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var list = await _repository.GetAllAsync();
                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Records Fetched";
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
