using AuthenticationManager;
using MediatR;
using Merchants.Application.Commands.Terminals;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Merchants.API.Controllers
{

    public class TerminalController : ApiController
    {
        private readonly IMediator _mediator;
        public TerminalController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //[RouteIdAuthorize("7")]
        [HttpGet("GetTerminals")]
        public async Task<ActionResult<Terminal>> GetTerminals()
        {
            var command = new GetAllTerminalsQuery();
            var result = await _mediator.Send(command);
                return Ok(result);
            
        }
        [HttpGet("GetTenants")]
        public async Task<ActionResult<Tenant>> GetTenants()
        {
            var command = new GetAllTenantsQuery();
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        //[RouteIdAuthorize("8")]
        [HttpPost(Name = "AddTerminal")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddTerminal([FromBody] AddTerminalCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            return Ok(result);
            else return BadRequest();
        }
        //[RouteIdAuthorize("9")]
        [HttpGet(Name= "GetTerminalBySerial")]
        public async Task<ActionResult<Response>> GetTerminalBySerial(string SerialNumber)
        {
            var command = new GetTerminalBySerialNumberQuery(SerialNumber);
            var result = await _mediator.Send(command);
            return result;
        }
        //[RouteIdAuthorize("20")]
        [HttpPost(Name = "UpdateTerminal")]
        public async Task<ActionResult<bool>> UpdateTerminal([FromBody] UpdateTerminalCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
        
        [HttpPost(Name = "GetTerminalById")]
        public async Task<ActionResult<Terminal>> GetTerminalById([FromBody] GetTerminalByIdCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
        //[RouteIdAuthorize("20")]
        [HttpPost(Name = "DeleteTerminal")]
        public async Task<ActionResult<bool>> DeleteTerminal([FromBody] DeleteTerminalCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TransactionType>>> GetTransactionTypes()
        {
            var command = new GetAllTransactionTypesQuery();
            var result = await _mediator.Send(command);
            return result.ToList();
        }
    }
}
