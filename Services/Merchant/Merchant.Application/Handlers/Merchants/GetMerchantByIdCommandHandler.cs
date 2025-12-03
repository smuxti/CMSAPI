using MediatR;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Merchants
{
    public class GetMerchantByIdCommandHandler : IRequestHandler<GetMerchantByIdCommand, Merchant>
    {
        private readonly IMerchantRepository _repo;

        public GetMerchantByIdCommandHandler(IMerchantRepository repo)
        {
            _repo = repo;
        }

        public async Task<Merchant> Handle(GetMerchantByIdCommand request, CancellationToken cancellationToken)
        {
            var merchant = await _repo.GetMerchantById(request.Id);
            return merchant;
        }
    }
}
