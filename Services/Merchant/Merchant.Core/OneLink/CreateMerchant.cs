namespace Merchants.Core.OneLink
{
    public class ContactDetails
    {
        public string phoneNo { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public string dept { get; set; }
        public string website { get; set; }
    }

    public class MerchantDetails
    {
        public string dbaName { get; set; }
        public string merchantName { get; set; }
        public string merchantID { get; set; }
        public string iban { get; set; }
        public string bankBic { get; set; }
        public string accountTitle { get; set; }
        public string merchantCategoryCode { get; set; }
        public PostalAddress postalAddress { get; set; }
        public ContactDetails contactDetails { get; set; }
        public PaymentDetails paymentDetails { get; set; }
    }

    public class PaymentDetails
    {
        public string feeType { get; set; }
        public string feeValue { get; set; }
    }

    public class PostalAddress
    {
        public string townName { get; set; }
        public string subDept { get; set; }
        public string addressLine { get; set; }
    }
    public class UpdateMerchantDetails
    {
        public string dbaName { get; set; }
        public string merchantName { get; set; }
        public string merchantID { get; set; }
        public string merchantStatus { get; set; }
        public string reasonCode { get; set; }
        public string iban { get; set; }
        public string bankBic { get; set; }
        public string accountTitle { get; set; }
        public string merchantCategoryCode { get; set; }
        public PostalAddress postalAddress { get; set; }
        public ContactDetails contactDetails { get; set; }
        public PaymentDetails paymentDetails { get; set; }
    }
    public class CreateMerchant
    {
        public MerchantDetails merchantDetails { get; set; }
    }
    public class UpdateMerchant
    {
        public UpdateMerchantDetails merchantDetails { get; set; }
    }
}
