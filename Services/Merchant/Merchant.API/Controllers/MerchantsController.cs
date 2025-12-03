using MediatR;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{
    public class MerchantsController : ApiController
    {
        private readonly IMediator _mediator;
        public MerchantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddMerchant")]
        public async Task<ActionResult<Response>> AddChannel([FromBody] AddMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateMerchant")]
        public async Task<ActionResult<Response>> UpdateChannel([FromBody] UpdateMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "DeleteMerchant")]
        public async Task<ActionResult<Response>> DeleteChannel([FromBody] DeleteMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetMerchantByID(int MerchantID)
        {
            var command = new GetMerchantByIDQuery(MerchantID);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMerchant()
        {
            var command = new GetAllMerchantQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
