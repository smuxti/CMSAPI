using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Entities
{
    [Table("tblBanks")]
    public class Bank
    {
        [Key]
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string BicIbanCode { get; set; }
    }
}
