using AuthenticationManager;
using MediatR;
using Merchants.Application.Commands.Banks;
using Merchants.Application.Commands.FeeSlabs;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Merchants.API.Controllers
{
    public class MerchantController : ApiController
    {
        private readonly IMediator _mediator;
        public MerchantController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[RouteIdAuthorize("2")]
        [HttpGet(Name = "GetMerchants")]
        [ProducesResponseType(typeof(IEnumerable<MerchantResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<MerchantResponse>>> GetMerchants(string name)
        {
            try
            {
                var query = new GetMerchantListQuery(name);
                var merchants = await _mediator.Send(query);
                return Ok(merchants);
            }
            catch (Exception ex)
            {

                return BadRequest("");
            }

        }
        //[RouteIdAuthorize("3")]
        [HttpPost(Name = "AddMerchant")]
        public async Task<ActionResult<Response>> AddMerchant([FromBody] AddMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        //[RouteIdAuthorize("4")]
        [HttpPost(Name = "UpdateMerchant")]
        public async Task<ActionResult<Response>> UpdateMerchant([FromBody] UpdateMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [RouteIdAuthorize("5")]
        [HttpPost(Name = "DeleteMerchant")]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> DeleteMerchant(Guid Id)
        {
            Guid UserId = Guid.Parse("0A7EB5DF-B507-4620-938A-E7CB493B465A");
            var command = new DeleteMerchantCommand() { Id = Id, UserId = UserId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        //[RouteIdAuthorize("6")]
        [HttpPost]
        public async Task<ActionResult<Response>> PostMerchant([FromBody] MerchantPostCommand command)
        {
            var resutl = await _mediator.Send(command);
            return Ok(resutl);
        }

        //[RouteIdAuthorize("18")]
        [HttpPost(Name = "GetBank")]
        public async Task<ActionResult<Response>> GetBank([FromBody] GetBankByIdCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //[RouteIdAuthorize("19")]
        [HttpGet(Name = "GetBanks")]
        public async Task<ActionResult<Response>> GetBanks()
        {
            GetAllBanksCommand command = new GetAllBanksCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet(Name = "GetSlabs")]
        public async Task<ActionResult<Response>> GetSlabs()
        {
            GetAllSlabsCommand command = new GetAllSlabsCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet(Name = "GetMerchantCategoryCode")]
        public async Task<ActionResult<MerchantCategory>> GetMerchantCategoryCode()
        {
            GetAllMerchantCategoryCodeCommand command = new GetAllMerchantCategoryCodeCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "GetMerchantById")]
        public async Task<ActionResult<Merchant>> GetMerchantById([FromBody] GetMerchantByIdCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "GetMerchantByEmail")]
        public async Task<ActionResult<Merchant>> GetMerchantByEmail([FromBody] GetMerchantByEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost(Name = "MerchantActivation")]
        public async Task<ActionResult<Response>> MerchantActivation([FromBody] MerchantActivationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost(Name = "SendActivationEmail")]
        public async Task<ActionResult<Response>> SendActivationEmail([FromBody] SendActivationEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
