using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{

    public class ComplaintDetailsController : ApiController
    {
        private readonly IMediator _mediator;
        public ComplaintDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddComplaintDetails")]
        public async Task<ActionResult<Response>> AddComplaintDetails([FromBody] AddCompaintDetailsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateComplaintDetails")]
        public async Task<ActionResult<Response>> UpdateComplaintDetails([FromBody] UpdateCompaintDetailsCommand command)
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
        [HttpPost(Name = "DeleteComplaintDetails")]
        public async Task<ActionResult<Response>> DeleteComplaintDetails([FromBody] DeleteCompaintDetailsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetComplaintDetails()
        {
            var command = new GetAllComplaintDetailsQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetComplaintDetailByID(int ID)
        {
            var command = new GetComplaintDetailByIDQuery(ID);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetByComplaintId(int ID)
        {
            var command = new GetComplaintDetailByComplaintIdCommand(ID);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
