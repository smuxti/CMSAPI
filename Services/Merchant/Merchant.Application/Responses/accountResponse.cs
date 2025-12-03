using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Responses
{
    public class accountResponse
    {
        public DateTime? Timestamp { get; set; }
        public string? Email { get; set; }
        public Guid Id { get; set; }
    }
}
