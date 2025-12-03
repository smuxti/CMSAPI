namespace EventNotificationManager.Model
{
    internal class TrenditSoundBox: EventBase
    {
        public string messageType { get; set; }
        public string orderNum { get; set; }
        public string type { get; set; }
        public string price { get; set; }
    }
}
