using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Common
{
    public class EventBase
    {
        public Guid EventId { get; private set; }
        public DateTime EventDate { get; private set; }
        public EventBase()
        {
            EventId = Guid.NewGuid();
            EventDate = DateTime.UtcNow;
        }
        public EventBase(Guid eventId, DateTime eventDate)
        {
            EventId = eventId;
            EventDate = eventDate;
        }
    }
}
