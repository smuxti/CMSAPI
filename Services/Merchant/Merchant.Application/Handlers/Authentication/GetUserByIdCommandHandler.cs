using Merchants.Application.Commands;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Authentication
{
    public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand,User>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<GetUserByIdCommandHandler> _logger;

        public GetUserByIdCommandHandler(IUserRepository repository, ILogger<GetUserByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<User> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserById(request.Id);
            return user;
        }
    }
}
