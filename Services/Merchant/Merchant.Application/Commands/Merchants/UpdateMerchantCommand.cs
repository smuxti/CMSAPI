using MediatR;
using Merchants.Application.Responses;

namespace Merchants.Application.Commands.Merchants
{
    public class UpdateMerchantCommand : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CommercialName { get; set; }
        public string ShortName { get; set; }
        public string CategoryCode { get; set; }
        public string IBAN { get; set; }
        public string BankBic { get; set; }
        public string AccountTitle { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string? Website { get; set; }
        public string FeeType { get; set; }
        public double FeeValue { get; set; }
        public Guid? SlabID { get; set; }
        public double? MinLimit { get; set; }
        public double? MaxLimit { get; set; }
        public string Status { get; set; }
        public string TenantCode { get; set; }
    }
}
