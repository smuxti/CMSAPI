

using Merchants.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Merchants.Core.Entities
{
    [Table("tblManagementHierarchy")]
    public class ManagementHierarchy:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string POCName { get; set; }
        public string Name { get; set; }
        public string POCEmail { get; set; }
        public string POCNumber { get; set; }
        public string OtherEmail { get; set; }
        public string OtherContact { get; set; }
        public string Address { get; set; }
        public int? ParentID { get; set; }
        public int ManagementType { get; set; }  ///0 for template records, 1 for Managment Herarchy, 2 for Area and 3 for Zone
        //public DateTime CreatedDate { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public int? UpdateBy { get; set; }
        //public DateTime? DeleteDate { get; set; }
        //public int? DeleteBy { get; set; }
        //public bool? IsDelete { get; set; }
        //public string Remarks { get; set; }
    }
}
