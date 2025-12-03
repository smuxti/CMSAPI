using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Handlers.ComplaintCategory;
using Merchants.Application.Queries;

//using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Merchants.API.Controllers
{

    public class ComplaintCategoryController : ApiController
    {


        private readonly IMediator _mediator;
        public ComplaintCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
  
        [HttpPost(Name = "AddComplaintCategory")]
        public async Task<ActionResult<Response>> AddComplaintCategory([FromBody] AddComplaintCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateComplaintCategory")]
        public async Task<ActionResult<Response>> UpdateComplaintCategory([FromBody] UpdateComplaintCategoryCommand command)
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
        [HttpPost(Name = "DeleteComplaintCategory")]
        public async Task<ActionResult<Response>> DeleteComplaintCategory([FromBody] DeleteComplaintCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetComplaintCategory()
        {
            var command = new GetAllComplaintCategoryQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult>GetComplaintCategoryByID(int id)
        {
            var command = new GetComplaintCategoryByIDQuery(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
