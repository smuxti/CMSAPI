using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Queries
{
    public class GetEquipmentByCategoryId : IRequest<Response>
    {
        public int CategoryId { get; set; }
        public GetEquipmentByCategoryId(int CategoryId) { this.CategoryId = CategoryId; }
    }
}
