using MediatR;
//using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Merchants.API.Controllers
{

    public class EscalationController : ApiController
    {
        private readonly IMediator _mediator;
        public EscalationController(IMediator mediator)
        {
            _mediator = mediator;
        }
   
        [HttpPost(Name = "AddEscalation")]
        public async Task<ActionResult<Response>> AddEscalation([FromBody] AddEscalationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateEscalation")]
        public async Task<ActionResult<Response>> UpdateEscalation([FromBody] UpdateEscalationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }




        [HttpPost(Name = "DeleteEscalation")]
        public async Task<ActionResult<Response>> DeleteEscalation([FromBody] DeleteEscalationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllEscalation()
        {
            var command = new GetAllEscalationQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllEscalationByCategory(int CategoryID,int Type)
        {
            var command = new GetAllEscalationByCategoryQuery(CategoryID,Type);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetEscalationByID(int MatrixID)
        {
            var command = new GetEscalationByIDQuery(MatrixID);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetByManagementId(int Id)
        {
            var command = new GetByManagementIdCommand(Id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Escalate()
        {
            var command = new AddEscalateCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
