using MediatR;
using Merchants.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Complaint
{
    public class AddCompaintCommand :IRequest<Response>
    {

        //public int ID { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public int TypeID { get; set; }
        public int CategoryID { get; set; }
        public int? MerchantID { get; set; }
        public int? EquipmentID { get; set; }
        public string? MerchantName { get; set; }
        public int ChannelID { get; set; }
        public string? Attachment { get; set; }

    }




}
