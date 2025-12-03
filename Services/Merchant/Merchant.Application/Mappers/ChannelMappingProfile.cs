using AutoMapper;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    //internal class ChannelMappingProfile
    //{
    //}


    public class ChannelMappingProfile : Profile
    {



        public ChannelMappingProfile()
        {
            //CreateMap<ComplaintCategory, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<Channel, AddChannelCommand>().ReverseMap();
            CreateMap<Channel, UpdateChannelCommand>().ReverseMap();
            CreateMap<Channel, DeleteChannelCommand>().ReverseMap();

        }

    }
}