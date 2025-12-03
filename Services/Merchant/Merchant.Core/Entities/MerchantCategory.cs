using Merchants.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Entities
{
    [Table("tblMerchantCategory")]
    public class MerchantCategory: BaseEntity
    {
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
    }
}
