using AutoMapper;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Responses;
using Merchants.Core.Entities;

namespace Merchants.Application.Mappers
{
    internal class MerchantMappingProfile:Profile
    {
        public MerchantMappingProfile()
        {
            CreateMap<Merchant, MerchantResponse>().ReverseMap();
            CreateMap<Merchant, AddMerchantCommand>().ReverseMap();
            CreateMap<Merchant, UpdateMerchantCommand>().ReverseMap();
            CreateMap<Merchant, DeleteMerchantCommand>().ReverseMap();
        }
    }
}
