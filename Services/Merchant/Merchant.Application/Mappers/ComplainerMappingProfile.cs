using AutoMapper;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class ComplainerMappingProfile:Profile
    {


        public ComplainerMappingProfile()
        {
            //CreateMap<ComplaintCategory, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<Complainer, AddComplainerCommand>().ReverseMap();
            CreateMap<Complainer, UpdateComplainerCommand>().ReverseMap();
            CreateMap<Complainer, DeleteComplainerCommand>().ReverseMap();
            //CreateMap<ComplaintCategory, GetAllComplaintCategoryCommand>().ReverseMap();


        }
    }
}
