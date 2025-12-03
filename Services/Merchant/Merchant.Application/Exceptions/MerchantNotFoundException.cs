namespace Merchants.Application.Exceptions
{
    internal class MerchantNotFoundException:ApplicationException
    {
        public MerchantNotFoundException(string name, object key):base($"Entity {name} - {key} is not found.")
        {
            
        }
    }
}
