using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetEscalationByIDQuery : IRequest<Response>
    {
        public int MatrixID { get; set; }
        public GetEscalationByIDQuery(int matrixID)
        {
            MatrixID = matrixID;
        }
    }
}
