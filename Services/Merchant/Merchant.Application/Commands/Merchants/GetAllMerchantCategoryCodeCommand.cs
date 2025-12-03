using MediatR;
using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Commands.Merchants
{
    public class GetAllMerchantCategoryCodeCommand : IRequest<IReadOnlyList<MerchantCategory>>
    {

    }
}
