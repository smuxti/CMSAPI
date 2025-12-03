using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Responses
{
    public class EquipmentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
