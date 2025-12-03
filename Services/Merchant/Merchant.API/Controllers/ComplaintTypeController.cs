using MediatR;
//using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{
    public class ComplaintTypeController : ApiController
    {
        private readonly IMediator _mediator;
        public ComplaintTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }
    
        [HttpPost(Name = "AddComplaintType")]
        public async Task<ActionResult<Response>> AddComplaintType([FromBody] AddCompaintTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateComplaintType")]
        public async Task<ActionResult<Response>> UpdateComplaintType([FromBody] UpdateCompaintTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "DeleteComplaintType")]
        public async Task<ActionResult<Response>> DeleteComplaintType([FromBody] DeleteCompaintTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetComplaintType()
        {
            var command = new GetAllComplaintTypeQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "ChangeStatus")]
        public async Task<ActionResult<Response>> ChangeStatus([FromBody] ChangeComplaintTypeStatus command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult>GetComplaintTypeByID(int id)
        {
            var command = new GetComplaintTypeByIDQuery(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
