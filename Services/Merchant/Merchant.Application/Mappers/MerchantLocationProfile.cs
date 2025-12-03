using AutoMapper;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Commands.MerchantLocation;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class MerchantLocationProfile : Profile
    {
        public MerchantLocationProfile()
        {
            //CreateMap<Complaint, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<ManagementHierarchy, AddMerchantLocationCommand>().ReverseMap();
            //CreateMap<Escalation, UpdateEscalationCommand>().ReverseMap();
            //CreateMap<Escalation, DeleteEscalationCommand>().ReverseMap();
            //CreateMap<IList<Escalation>, GetEscalationByCategoryCommand>().ReverseMap();

        }
    }
}
