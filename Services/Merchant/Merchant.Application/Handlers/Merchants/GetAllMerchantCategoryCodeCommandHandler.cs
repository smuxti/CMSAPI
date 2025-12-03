using MediatR;
using Merchants.Application.Commands.Merchants;
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
    public class GetAllMerchantCategoryCodeCommandHandler : IRequestHandler<GetAllMerchantCategoryCodeCommand,IReadOnlyList<MerchantCategory>>
    {
        private readonly IMerchantRepository _repository;
        private readonly ILogger<GetAllMerchantCategoryCodeCommandHandler> _logger;

        public GetAllMerchantCategoryCodeCommandHandler(IMerchantRepository repository,ILogger<GetAllMerchantCategoryCodeCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<MerchantCategory>> Handle(GetAllMerchantCategoryCodeCommand request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetAllMerchantCategories();
            return list.ToList();
        }
    }
}
