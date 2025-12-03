using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Merchants.Infrastructure.Repositories
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly ILogger<NotificationRepo> _logger;
        

        public NotificationRepo(IConfiguration configuration, HttpClient client, ILogger<NotificationRepo> logger)
        {
            _configuration = configuration;
            _client = client;
            _logger = logger;
        }
        public async Task<string> NotificationToManagement(string managementid,string message)
        {
            try
            {
                var _baseUrl = _configuration.GetValue<string>("CmsPortal");
                var response = await _client.GetAsync($"{_baseUrl}/Account/SendNotification?userid={managementid}&message={message}");
                var error = await response.Content.ReadAsStringAsync();

                return "Noti Send";

            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
    }
}
