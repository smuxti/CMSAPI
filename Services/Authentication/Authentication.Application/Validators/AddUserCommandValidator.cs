using Authentication.Application.Commands;
using FluentValidation;

namespace Authentication.Application.Validators
{
    internal class AddUserCommandValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserCommandValidator()
        {
            RuleFor(r => r.FirstName).NotEmpty().WithMessage("{First Name} is required")
                .NotNull()
                .MinimumLength(3).WithMessage("{First Name} msut not less than 3 characters.")
                .MaximumLength(25).WithMessage("{First Name} msut not exceed 25 characters.");
            RuleFor(r => r.LastName).NotEmpty().WithMessage("{Last Name} is required")
                .NotNull()
                .MinimumLength(3).WithMessage("{Last Name} msut not less than 3 characters.")
                .MaximumLength(25).WithMessage("{Last Name} msut not exceed 25 characters.");
            RuleFor(r => r.Username).NotEmpty().WithMessage("{UserName} is required")
                .NotNull()
                .MinimumLength(3).WithMessage("{UserName} msut not less than 3 characters.")
                .MaximumLength(25).WithMessage("{UserName} msut not exceed 25 characters.");
            RuleFor(r => r.Passowrd).NotEmpty().WithMessage("{Passowrd} is required")
                .NotNull();
            RuleFor(r => r.Email).NotEmpty().WithMessage("{Email} is required")
                .NotNull()
                .EmailAddress();
            RuleFor(r => r.MobileNumber)
                .NotEmpty()
                .WithMessage("{Mobile Number} is required")
                .NotNull();


        }
    }
}
