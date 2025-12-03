using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchants.Core.Entities
{
    [Table("tblTransactionTypes")]
    public class TransactionType : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public TransactionType()
        {
            Status = "Active";
        }
    }
}
