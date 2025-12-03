using MediatR;

namespace Merchants.Application.Commands.Merchants
{
    public class DeleteMerchantCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

    }
}
