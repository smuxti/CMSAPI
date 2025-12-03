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
    public class GetAllResourcesByMerchIdCommandHandler : IRequestHandler<GetAllResourcesByMerchIdCommand,IReadOnlyList<UserResource>>
    {
        private readonly IUserResourceRepository _repository;
        private readonly ILogger<GetAllResourcesByMerchIdCommandHandler> _logger;

        public GetAllResourcesByMerchIdCommandHandler(IUserResourceRepository repository,ILogger<GetAllResourcesByMerchIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<UserResource>> Handle(GetAllResourcesByMerchIdCommand request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetUserResourcesByMerchId(request.MerchantId);
            return list.ToList();
        }
    }
}
