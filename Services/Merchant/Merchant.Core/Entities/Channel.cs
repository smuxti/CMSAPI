
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Merchants.Core.Common;

namespace Merchants.Core.Entities
{
    [Table("tblChannel")]
    public class Channel:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ChannelType { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public int? UpdateBy { get; set; }
        //public DateTime? DeleteDate { get; set; }
        //public int DeleteBy { get; set; }
        //public bool? IsDelete { get; set; }
        //public string? Remarks { get; set; }
    }
}
