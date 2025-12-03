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
    public class DeleteUserTypeQueryHandler : IRequestHandler<DeleteUserTypeQuery, Response>
    {
        private readonly IUserTypeRepository _repository;
        private readonly ILogger<DeleteUserTypeQueryHandler> _logger;

        public DeleteUserTypeQueryHandler(IUserTypeRepository repository,ILogger<DeleteUserTypeQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Response> Handle(DeleteUserTypeQuery request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var model = await _repository.GetByGuidId(request.id);

                if (model?.Locked ?? false)
                {
                    baseResponse.isSuccess = false;
                    baseResponse.ResponseDescription = "Record is Locked";
                    baseResponse.ResponseCode = 0;
                    baseResponse.Data = null;
                    return baseResponse;
                }

                await _repository.DeleteHardAsync(model);
                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Record Deleted";
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
