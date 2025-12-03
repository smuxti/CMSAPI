namespace EventNotificationManager.Model
{
    internal class MorefunSoundBox : EventBase
    {
        public int broadcast_type { get; set; }
        public string money { get; set; }
        public string request_id { get; set; }
        public string datetime { get; set; }
        public long ctime { get; set; }
        public MorefunSoundBox()
        {
            broadcast_type = 1;
            request_id = DateTimeOffset.Now.ToString("yyyyMMddhhmmssmi");
            datetime = DateTimeOffset.Now.ToString("yyyyMMddhhmmssmi");
            ctime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
