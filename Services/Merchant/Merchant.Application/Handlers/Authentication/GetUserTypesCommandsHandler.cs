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
    public class GetUserTypesCommandsHandler : IRequestHandler<GetUserTypesCommands,IReadOnlyList<UserType>>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<GetUserTypesCommandsHandler> _logger;

        public GetUserTypesCommandsHandler(IUserRepository repository, ILogger<GetUserTypesCommandsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<UserType>> Handle(GetUserTypesCommands request, CancellationToken cancellationToken)
        {
            var userTypes = await _repository.GetUserTypes();
            return userTypes.ToList();
        }
    }
}
