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
    public class GetAllUserCommandHandler : IRequestHandler<GetAllUserCommand,IReadOnlyList<User>>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<GetAllUserCommandHandler> _logger;

        public GetAllUserCommandHandler(IUserRepository repository, ILogger<GetAllUserCommandHandler> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        public async Task<IReadOnlyList<User>> Handle(GetAllUserCommand request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetAllAsync();
            return list.Where(x => x.isDeleted == false).ToList();
        }
    }
}
