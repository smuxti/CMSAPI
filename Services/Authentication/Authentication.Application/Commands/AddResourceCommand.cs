using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Commands
{
    public class AddResourceCommand : IRequest<bool>
    {
        [FromForm]
        public Guid MerchantId { get; set; }
        [FromForm]
        public string FileType { get; set; }
        [FromForm]
        public IFormFile File { get; set; }
    }
}
