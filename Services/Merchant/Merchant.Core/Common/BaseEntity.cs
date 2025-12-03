using System.Text.Json.Serialization;

namespace Merchants.Core.Common
{
    public abstract class BaseEntity
    {
        //public Guid Id { get; set; }
         
        public Guid? CreatedBy { get; set; }
        
        public DateTime? CreatedAt { get; set; }
       
        public Guid? UpdatedBy { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public Guid? DeletedBy { get; set; }
        
        public DateTime? DeletedAt { get; set; }
        
        public bool isDeleted { get; set; }

        public string Status { get; set; }
        public string? Remarks { get; set; }
    }
}
