using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.ManagementHierarchy
{
    public class AddManagementHierarchyCommand:IRequest<Response>
    {
        public string POCName { get; set; }
        public string Name { get; set; }
        public string POCEmail { get; set; }
        public string POCNumber { get; set; }
        public string OtherEmail { get; set; }
        public string OtherContact { get; set; }
        public string Address { get; set; }
        public int? ParentID { get; set; }
        public int ManagementType { get; set; }
    }


}
