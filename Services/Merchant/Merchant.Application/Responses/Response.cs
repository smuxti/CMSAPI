namespace Merchants.Application.Responses
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public bool isSuccess { get; set; }
        public string ResponseDescription { get; set; }
        public dynamic Data { get; set; }
    }
}
