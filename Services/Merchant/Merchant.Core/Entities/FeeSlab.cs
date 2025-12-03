using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merchants.Core.Entities
{
    [Table("tblFeeSlab")]
    public class FeeSlab: BaseEntity
    {
        public string Name { get; set; }
        public string SlabType { get; set; }

    }
}
