using MediatR;

namespace Merchants.Application.Commands.Terminals
{
    public class AddTerminalCommand: IRequest<bool>
    {
        public string SerialNumber { get; set; }
        public Guid MerchantID { get; set; }
        public string DeviceType { get; set; }
        public string? IBAN { get; set; }
        public string? BankBic { get; set; }
        public string? AccountTitle { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string? Website { get; set; }
        public string? FeeType { get; set; }
        public double? FeeValue { get; set; }
        public Guid? SlabID { get; set; }
        public double MinLimit { get; set; }
        public double MaxLimit { get; set; }
        public string TerminalCode { get; set; }
        public string Status { get; set; }
    }
}
