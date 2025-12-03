using Authentication.Application.Commands;
using Authentication.Core.Entities;
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
            var user = await _repository.GetById(request.Id);
            return user;
        }
    }
}
