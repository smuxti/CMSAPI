using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.OneLink
{

    public class Info
    {
        public string merchantID { get; set; }
    }

    public class CreateMerchantOneLinkResponse
    {
        public string responseCode { get; set; }
        public string responseDesc { get; set; }
        public Info info { get; set; }
    }
}
