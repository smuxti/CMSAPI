using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Merchants.Application.Responses
{
    public class ComplaintViewModel
    {
        public int ID { get; set; }
        public DateTime ComplaintDate { get; set; }
        public int ComplainerID { get; set; }
        public string ComplaintDetail { get; set; }

        public string Description { get; set; }
        public int? TypeID { get; set; }
        public string TypeName { get; set; }
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }


        public int MerchantID { get; set; }
        public string MerchantName { get; set; }

        public int ChannelID { get; set; }
        public string ChannelName { get; set; }

        public string? Status { get; set; }
        public string? Remarks { get; set; }

        

    }
}
