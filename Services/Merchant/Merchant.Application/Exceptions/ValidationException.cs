using FluentValidation.Results;

namespace Merchants.Application.Exceptions
{
    internal class ValidationException: ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public ValidationException():base("One or more error(s) occured.")
        {
            Errors = new Dictionary<string, string[]>();    
        }
        public ValidationException(IEnumerable<ValidationFailure> failures) :this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(f => f.Key, f => f.ToArray());
        }
    }
}
