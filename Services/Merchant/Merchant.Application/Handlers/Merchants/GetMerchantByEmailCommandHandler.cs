using MediatR;
using Merchants.Application.Commands.Merchants;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Merchants
{
    public class GetMerchantByEmailCommandHandler : IRequestHandler<GetMerchantByEmailCommand, Merchant>
    {
        private readonly IMerchantRepository _repo;

        public GetMerchantByEmailCommandHandler(IMerchantRepository repo)
        {
            _repo = repo;
        }

        public async Task<Merchant> Handle(GetMerchantByEmailCommand request, CancellationToken cancellationToken)
        {
            var merchant = await _repo.GetMerchantByEmail(request.Email);
            return merchant;
        }
    }
}
