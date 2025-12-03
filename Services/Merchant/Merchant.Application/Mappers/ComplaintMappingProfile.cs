using AutoMapper;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Handlers.Complaint;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class ComplaintMappingProfile: Profile
    {


        public ComplaintMappingProfile()
        {
            //CreateMap<Complaint, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<Complaint, AddCompaintCommand>().ReverseMap();
            CreateMap<Complaint, DeleteCompaintCommand>().ReverseMap();
            CreateMap<Complaint, UpdateCompaintCommand>().ReverseMap();
            CreateMap<Complaint, UpdateCompaintCommand>().ReverseMap();


        }
    }
}
