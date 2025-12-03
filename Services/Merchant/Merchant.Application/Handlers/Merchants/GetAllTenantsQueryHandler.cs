using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Merchants
{
    public class GetAllTenantsQueryHandler : IRequestHandler<GetAllTenantsQuery, IReadOnlyList<Tenant>>
    {
        private readonly ITerminalRepository _terminalRepository;
        private readonly ILogger<GetAllTenantsQueryHandler> _logger;

        public GetAllTenantsQueryHandler(ITerminalRepository terminalRepository, ILogger<GetAllTenantsQueryHandler> logger)
        {
            _terminalRepository = terminalRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Tenant>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            var tenants=await _terminalRepository.GetAllTenants();
            return tenants.ToList();
        }
    }
}
