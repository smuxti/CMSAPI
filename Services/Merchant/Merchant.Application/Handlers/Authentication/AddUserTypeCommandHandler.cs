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

namespace Merchants.Application.Handlers
{
    public class AddUserTypeCommandHandler : IRequestHandler<AddUserTypeCommand, Response>
    {
        private readonly ILogger<AddUserTypeCommandHandler> _logger;
        private readonly IUserTypeRepository _repository;

        public AddUserTypeCommandHandler(ILogger<AddUserTypeCommandHandler> logger, IUserTypeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response> Handle(AddUserTypeCommand request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            try
            {
                var existingType = (await _repository.GetAllAsync(x => x.TypeName == request.TypeName || x.TypeCode == request.TypeCode)).FirstOrDefault();
                if (existingType != null)
                {
                    baseResponse.isSuccess = false;
                    baseResponse.ResponseDescription = "Record Already Exist";
                    baseResponse.ResponseCode = 0;
                    baseResponse.Data = null;
                    return baseResponse;
                }

                UserType model = new UserType();
                model.TypeName = request.TypeName;
                model.TypeCode= request.TypeCode;

                await _repository.AddAsync(model);
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
