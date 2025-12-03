using Merchants.Application.Commands;
using FluentValidation;

namespace Merchants.Application.Validators
{
    internal class AddUserCommandValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserCommandValidator()
        {
            RuleFor(r => r.Username).NotEmpty().WithMessage("{User Name} is required")
                .NotNull()
                .MinimumLength(3).WithMessage("{User Name} msut not less than 3 characters.")
                .MaximumLength(25).WithMessage("{User Name} msut not exceed 25 characters.");
            RuleFor(r => r.ManagementId)
                .NotEmpty().NotNull().WithMessage("{Management Id} is required") // Ensure it's not zero (default for int)
                .GreaterThan(0).WithMessage("ManagementId must be greater than 0");
            RuleFor(r => r.UserTypeCode)
                .NotEmpty().NotNull().WithMessage("{User Type Code} is required") // Ensure it's not zero (default for int)
                .GreaterThan(0).WithMessage("UserTypeCode must be greater than 0");
           


        }
    }
}
