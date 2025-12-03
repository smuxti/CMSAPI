using EventNotificationManager.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
namespace EventNotificationManager.Event
{
    public class EventMQTT
    {
        //private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly MQTTConfig _mqttOptions;
        public EventMQTT(IOptions<MQTTConfig> mqttOptions,/*IConfiguration configuration,*/ ILogger<EventMQTT> logger)
        {
            //_configuration = configuration;
            _logger = logger;
            _mqttOptions = mqttOptions.Value;
            _logger.LogInformation($"_mqttOptions: {_mqttOptions}");
        }
        public async Task<bool> MQTTSender(Guid EventId, DateTime EventDate, string message, string topic, string deviceType)
        {
            try
            {
                _logger.LogInformation($"Message receive to proceed with MQTT Brocker against EventId {EventId} at EventDate {EventDate} Topic {topic} & Device Type {deviceType}.");
                string _msg = string.Empty;
                if (deviceType.ToLower().Equals("trendit"))
                {
                    TrenditSoundBox soundbox = new TrenditSoundBox()
                    {
                        price = message,
                        messageType = "",
                        type = "",
                        orderNum = ""
                    };
                    _msg = JsonSerializer.Serialize(soundbox);
                }
                if (deviceType.ToLower().Equals("morefun"))
                {
                    MorefunSoundBox soundbox = new MorefunSoundBox()
                    {
                        money = message
                    };
                    _msg = JsonSerializer.Serialize(soundbox);
                }
                else
                {
                    _msg = message;
                }
                string _mqttServer = _mqttOptions.Server;//, _configuration["MQTT:Server"];
                string _mqttUsername = _mqttOptions.Username;// _configuration["MQTT:Username"];
                string _mqttPassword = _mqttOptions.Password;//_configuration["MQTT:Password"];
                string _mqttPort = _mqttOptions.Port;
                
                IMqttClient mqttClient;
                var ClientID = Guid.NewGuid().ToString();
                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();
                if (!string.IsNullOrEmpty(_mqttUsername))
                {
                    var options = new MqttClientOptionsBuilder()
                        .WithTcpServer(_mqttServer,Convert.ToInt32(_mqttPort))
                        .WithCredentials(_mqttUsername, _mqttPassword)
                        .WithKeepAlivePeriod(TimeSpan.FromSeconds(5))
                        .WithClientId(ClientID)
                        .WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .Build();

                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                else
                {
                    var options = new MqttClientOptionsBuilder()
                        .WithTcpServer(_mqttServer, Convert.ToInt32(_mqttPort))
                        .WithKeepAlivePeriod(TimeSpan.FromSeconds(5))
                        .WithClientId(ClientID)
                        .Build();

                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }

                _logger.LogInformation($"Message: {_msg}");
                _logger.LogInformation($"Connection establised wuth MQTT Brocker against EventId {EventId} at EventDate {EventDate}.");
                var applicationMessage = new MqttApplicationMessageBuilder()
                  .WithTopic(topic)
                  .WithPayload(_msg)
                  .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                  .Build();
                await mqttClient.PublishAsync(applicationMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Connection failed wuth MQTT Brocker against EventId {EventId} at EventDate {EventDate} Error {ex.Message} Inner Msg {ex.InnerException}.");
                return false;
            }
        }
    }
}
