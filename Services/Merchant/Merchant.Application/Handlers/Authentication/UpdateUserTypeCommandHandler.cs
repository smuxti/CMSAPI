using Merchants.Application.Commands;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
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
    public class UpdateUserTypeCommandHandler : IRequestHandler<UpdateUserTypeCommand, Response>
    {
        private readonly ILogger<UpdateUserTypeCommandHandler> _logger;
        private readonly IUserTypeRepository _repository;

        public UpdateUserTypeCommandHandler(ILogger<UpdateUserTypeCommandHandler> logger, IUserTypeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response> Handle(UpdateUserTypeCommand request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var model = await _repository.GetByGuidId(request.Id);
                model.TypeName = request.TypeName;
                model.TypeCode = request.TypeCode;

                await _repository.UpdateAsync(model);
                baseResponse.isSuccess = true;
                baseResponse.ResponseDescription = "Record Updated";
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
