namespace Merchants.Application.Responses
{
    public class TerminalResponse
    {
        public string SerialNumber { get; set; }
        public string MerchantCode { get; set; }
        public string DeviceType { get; set; }
        public string TerminalCode { get; set; }
        public string MerchantName { get; set; }
        public double MaxLimit { get; set; }
        public double MinLimit { get; set; }
        public string IBAN { get; set; }
    }
}
