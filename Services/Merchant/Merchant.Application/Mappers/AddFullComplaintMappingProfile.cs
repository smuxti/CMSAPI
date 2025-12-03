using AutoMapper;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    //internal class AddFullComplaintMappingProfile
    //{
    //}


    public class AddFullComplaintMappingProfile : Profile
    {

        public AddFullComplaintMappingProfile()
        {
            CreateMap<Complaint, AddCompaintCommand>().ReverseMap();
            CreateMap<Complainer, AddComplainerCommand>().ReverseMap();
            CreateMap<ComplaintDetails, AddCompaintDetailsCommand>().ReverseMap();
            CreateMap<Complaint, ComplaintViewModel>().ReverseMap();

        }

    }

}
