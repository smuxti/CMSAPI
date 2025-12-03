using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Complaint
{


    public class UpdateCompaintCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public int ComplainerID { get; set; }
        public string ComplaintDetail { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string Description { get; set; }
        public int TypeID { get; set; }
        public int CategoryID { get; set; }
        public int MerchantID { get; set; }
        public string? MerchantName { get; set; }
        public int ChannelID { get; set; }
        public bool? Status { get; set; }
        //public DateTime? ResolvedDate { get; set; }
        //public int? SatisfactionScore { get; set; }
        //public string? OTP { get; set; }
        public string? Remarks { get; set; }

    }
}

