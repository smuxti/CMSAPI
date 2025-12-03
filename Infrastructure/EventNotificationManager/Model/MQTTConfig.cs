namespace EventNotificationManager.Model
{
    public class MQTTConfig
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public bool isSSL { get; set; }
    }
}
