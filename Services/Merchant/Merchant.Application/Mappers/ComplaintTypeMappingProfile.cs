using AutoMapper;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Handlers.Complaint;
using Merchants.Application.Handlers.ComplaintType;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class ComplaintTypeMappingProfile:Profile
    {
        public ComplaintTypeMappingProfile()
        {
            //CreateMap<Complaint, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<ComplaintType, AddCompaintTypeCommand>().ReverseMap();
            CreateMap<ComplaintType, UpdateCompaintTypeCommand>().ReverseMap();
            CreateMap<ComplaintType, DeleteCompaintTypeCommand>().ReverseMap();
            CreateMap<ComplaintType, ChangeComplaintTypeStatus>().ReverseMap();

        }
    }


  
}
