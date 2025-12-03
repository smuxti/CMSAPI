using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchants.Core.Entities
{
    [Table("tblFeeSlabDetail")]
    public class FeeSlabDetail:BaseEntity
    {
        public Guid SlabID { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double FixedFee { get; set; }
        public double FeeValue { get; set; }

    }
}
