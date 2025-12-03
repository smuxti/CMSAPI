using MediatR;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{

    public class ChannelController : ApiController
    {

        private readonly IMediator _mediator;
        public ChannelController(IMediator mediator)
        {
            _mediator = mediator;
        }
   
        [HttpPost(Name = "AddChannel")]
        public async Task<ActionResult<Response>> AddChannel([FromBody] AddChannelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateChannel")]
        public async Task<ActionResult<Response>> UpdateChannel([FromBody] UpdateChannelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "DeleteChannel")]
        public async Task<ActionResult<Response>> DeleteChannel([FromBody] DeleteChannelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult> GetChannel()
        {
            var command = new GetAllChannelQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetChannelByID(int id)
        {
            var command = new GetChannelByIDQuery(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
