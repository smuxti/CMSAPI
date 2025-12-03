using MediatR;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Commands.MerchantLocation;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Mvc;


namespace Merchants.API.Controllers
{
    public class MerchantLocationController : ApiController
    {
        private readonly IMediator _mediator;
        public MerchantLocationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(Name = "AddMerchantLocation")]
        public async Task<ActionResult<Response>> AddMerchantLocation([FromBody] AddMerchantLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "UpdateMerchantLocation")]
        public async Task<ActionResult<Response>> UpdateMerchantLocation([FromBody] UpdateMerchantLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost(Name = "DeleteMerchantLocation")]
        public async Task<ActionResult<Response>> DeleteMerchantLocation([FromBody] DeleteMerchantLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet(Name ="GetZones")]
        public async Task<ActionResult<Response>> GetMerchantZones()
        {
            var command = new GetZonesCommand();
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpGet(Name ="GetZoneAreas")]
        public async Task<ActionResult<Response>> GetZoneAreas(int ZoneID)
        {
            var command = new GetZoneByIDQuery(ZoneID);

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
