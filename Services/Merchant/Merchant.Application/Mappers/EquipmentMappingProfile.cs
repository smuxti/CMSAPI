using AutoMapper;
using Merchants.Application.Commands.Equipment;
using Merchants.Application.Responses;
using Merchants.Core.Entities;

namespace Merchants.Application.Mappers
{
    public class EquipmentMappingProfile : Profile
    {
        public EquipmentMappingProfile()
        {
            CreateMap<Equipment, AddEquipmentCommand>().ReverseMap();
            CreateMap<Equipment, EquipmentResponse>().ReverseMap();
        }

    }
}
