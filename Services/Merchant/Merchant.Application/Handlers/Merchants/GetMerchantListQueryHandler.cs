using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;

namespace Merchants.Application.Handlers.MerchantHandlers
{
    internal class GetMerchantListQueryHandler : IRequestHandler<GetMerchantListQuery, List<MerchantResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepository;
        public GetMerchantListQueryHandler(IMerchantRepository merchantRepository, IMapper mapper)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
        }
        public async Task<List<MerchantResponse>> Handle(GetMerchantListQuery request, CancellationToken cancellationToken)
        {
            var merchantList = await _merchantRepository.GetMerchantByName(request.Name);
            return _mapper.Map<List<MerchantResponse>>(merchantList);
        }
    }
}
