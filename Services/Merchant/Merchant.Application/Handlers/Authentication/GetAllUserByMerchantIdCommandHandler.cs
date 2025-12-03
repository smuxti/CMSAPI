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
    public class GetAllUserByMerchantIdCommandHandler : IRequestHandler<GetAllUserByMerchantIdCommand, IReadOnlyList<User>>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<GetAllUserByMerchantIdCommandHandler> _logger;

        public GetAllUserByMerchantIdCommandHandler(IUserRepository repository, ILogger<GetAllUserByMerchantIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<User>> Handle(GetAllUserByMerchantIdCommand request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllUserByMerchantId(request.Id);
            return users.Where(x=>x.isDeleted==false).ToList();
        }
    }
}
