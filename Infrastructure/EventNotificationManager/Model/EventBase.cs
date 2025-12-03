using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventNotificationManager.Model
{
    public class EventBase
    {
        public Guid Id { get; private set; }
        public DateTime EventDate { get; private set; }
        public EventBase()
        {
            Id= Guid.NewGuid();
            EventDate = DateTime.UtcNow;
        }
        public EventBase(Guid id, DateTime eventDate)
        {
            id = Id;
            eventDate = EventDate;
        }
    }
}
