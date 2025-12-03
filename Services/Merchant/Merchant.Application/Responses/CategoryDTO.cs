using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Responses
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string? AltName { get; set; }
        public int Type { get; set; }
        public bool? HasEquipment { get; set; }
    }
}
