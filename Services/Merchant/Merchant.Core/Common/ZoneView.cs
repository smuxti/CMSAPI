using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Common
{
    public class ZoneView
    {
        public int ID { get;set; }
        public string? Location { get; set; }    
        public string? POCName { get; set; }
        public string? POCEmail { get; set; }
        public string? POCNumber { get; set; }
    }
}
