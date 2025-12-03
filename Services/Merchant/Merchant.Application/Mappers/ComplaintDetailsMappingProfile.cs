using AutoMapper;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class ComplaintDetailsMappingProfile:Profile
    {


        public ComplaintDetailsMappingProfile()
        {
            //CreateMap<ComplaintCategory, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<ComplaintDetails, AddCompaintDetailsCommand>().ReverseMap();
            CreateMap<ComplaintCategory, UpdateCompaintDetailsCommand>().ReverseMap();
            CreateMap<ComplaintCategory, DeleteCompaintDetailsCommand>().ReverseMap();
            //CreateMap<ComplaintCategory, GetAllComplaintCategoryCommand>().ReverseMap();


        }

    }
}
