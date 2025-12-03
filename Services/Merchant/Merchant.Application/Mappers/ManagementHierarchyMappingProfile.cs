using AutoMapper;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Mappers
{
    public class ManagementHierarchyMappingProfile:Profile
    {
        public ManagementHierarchyMappingProfile()
        {
            //CreateMap<Complaint, ComplaintCategoryResponse>().ReverseMap();
            CreateMap<ManagementHierarchy, AddManagementHierarchyCommand>().ReverseMap();
            CreateMap<ManagementHierarchy, UpdateManagementHierarchyCommand>().ReverseMap();
            CreateMap<ManagementHierarchy, DeleteManagementHierarchyCommand>().ReverseMap();

        }
    }

    
}
