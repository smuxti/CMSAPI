using Authentication.Application.Commands;
using FluentValidation;

namespace Authentication.Application.Validators
{
    internal class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
    }
}
