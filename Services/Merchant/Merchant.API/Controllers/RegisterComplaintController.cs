using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{
    
    public class RegisterComplaintController : ApiController
    {

        private readonly IMediator _mediator;
        public RegisterComplaintController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("AddFullComplaint")]
        public async Task<ActionResult<Response>> AddFullComplaint([FromBody] AddFullComplaintCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }




    }
}
