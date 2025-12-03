using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchants.Core.Entities
{
    [Table("tblMerchantTransactionTypes")]
    public class MerchantTransactionType: BaseEntity
    {
        public Guid MerchantID { get; set; }
        public int TransactionTypeCode { get; set; }
    }
}
