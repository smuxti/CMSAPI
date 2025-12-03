using AutoMapper;
//using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Responses;

//using Merchants.Application.Commands.Merchants;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class ComplaintCategoryMappingProfile: Profile
    {

       

            public ComplaintCategoryMappingProfile()
            {
                //CreateMap<ComplaintCategory, ComplaintCategoryResponse>().ReverseMap();
                CreateMap<ComplaintCategory, AddComplaintCategoryCommand>().ReverseMap();
                CreateMap<ComplaintCategory, UpdateComplaintCategoryCommand>().ReverseMap();
                CreateMap<ComplaintCategory, DeleteComplaintCategoryCommand>().ReverseMap();
                CreateMap<ComplaintCategory, CategoryDTO>().ReverseMap();
                 //CreateMap<ComplaintCategory, GetAllComplaintCategoryCommand>().ReverseMap();


        }



    }
}
