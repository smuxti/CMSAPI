using AutoMapper.Execution;
using MediatR;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{
   
    public class ComplainerController : ApiController
    {

        private readonly IMediator _mediator;
        public ComplainerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddComplainer")]
        public async Task<ActionResult<Response>> AddComplainer([FromBody] AddComplainerCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateComplainer")]
        public async Task<ActionResult<Response>> UpdateComplainer([FromBody] UpdateComplainerCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        //[HttpPost(Name = "GetComplaintCategoryById")]
        //public async Task<ActionResult> GetTerminalById([FromBody] GetComplaintCategoryIdCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return result;
        //}
        //[RouteIdAuthorize("20")]
        [HttpPost(Name = "DeleteComplainer")]
        public async Task<ActionResult<Response>> DeleteComplainer([FromBody] DeleteComplainerCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetComplaintCategory()
        {
            var command = new GetAllComplainerQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> TestEmail(string Email)
        {
            var command = new TestQuery(Email);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetComplainerByID(int id)
        {
            var command = new GetComplainerByIDQuery(id);
            var result =await _mediator.Send(command);
            return Ok(result);
        }
    }
}
