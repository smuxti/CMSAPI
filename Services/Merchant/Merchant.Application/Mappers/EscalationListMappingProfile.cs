using AutoMapper;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Commands.Escalation;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class EscalationListMappingProfile : Profile
    {
        public EscalationListMappingProfile()
        {
            //CreateMap<Complaint, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<Escalation, EscalationList>().ReverseMap();
            CreateMap<Escalation, UpdateEscalationCommand>().ReverseMap();
            CreateMap<Escalation, DeleteEscalationCommand>().ReverseMap();

        }
    }
}
