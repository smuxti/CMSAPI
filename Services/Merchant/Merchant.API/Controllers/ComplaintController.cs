using AutoMapper.Execution;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Infrastructure.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{

    public class ComplaintController : ApiController
    {
        private readonly IMediator _mediator;
        public ComplaintController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddComplaint")]
        public async Task<ActionResult<Response>> AddComplaint([FromBody] AddCompaintCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateComplaint")]
        public async Task<ActionResult<Response>> UpdateComplaint([FromBody] UpdateCompaintCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "ForwardComplaint")]
        public async Task<ActionResult<Response>> ForwardComplaint([FromBody] ForwardComplaintCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "DeleteComplaint")]
        public async Task<ActionResult<Response>> DeleteComplaint([FromBody] DeleteCompaintCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllComplaints()
        {
            var command = new GetAllComplaintsQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("addFullComplaint")]
        public async Task<ActionResult<Response>> AddFullComplaint([FromBody] AddFullComplaintCommand command)
        {
            try
            {
                // Call the mediator to handle the command
                var result = await _mediator.Send(command);

                // Return success response
                if (result.isSuccess)
                    return Ok(result);

                // Return failure response
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                // Log error and return failure response
                return StatusCode(500, new Response
                {
                    isSuccess = false,
                    ResponseCode = 0,
                    ResponseDescription = $"An error occurred: {ex.Message}",
                });
            }
        }
        [HttpGet(Name = "ComplaintHistory")]



        public async Task<ActionResult<Response>> GetComplaintHistory(int id)
        {
            var query = new GetComplaintHistoryQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Name = "GetById")]
        public async Task<ActionResult<Response>> GetById(int id)
        {
            var command = new GetComplaintByIdCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "StartComplaint")]
        public async Task<ActionResult<Response>> StartComplaint([FromBody] StartComplaintCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Name = "UpdateComplaintStatus")]
        public async Task<ActionResult<Response>> UpdateComplaintStatus([FromBody] UpdateComplaintStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost(Name = "GetComplaintByManagementID")]
        public async Task<ActionResult<Response>> GetComplaintByManagmentID(int ManagmentID)
        {
            var query = new GetComplaintByManagmentIDQuery(ManagmentID);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Name = "GetYearlyComplaints")]
        public async Task<ActionResult<int>> GetYearlyComplaintsCount([FromQuery] GetYearlyComplaintsCountQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet(Name = "GetWeeklyComplaints")]
        public async Task<ActionResult<int>> GetWeeklyComplaintsCount([FromQuery] GetWeeklyComplaintsCountQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet(Name = "GetMonthlyStatus")]
        public async Task<ActionResult<int>> GetMonthlyStatusCount([FromQuery] GetComplaintStatusCountQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet(Name = "GetTopCategory")]
        public async Task<ActionResult<int>> GetTopCategory([FromQuery] GetTopCategoriesQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet(Name = "GetComplainCountToday")]
        public async Task<ActionResult<Response>> GetComplainCountToday() 
        {
            var query = new GetComplainCount();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Name = "GetPendingComplain")]
        public async Task<ActionResult<Response>> GetPendingComplain()
        {
            var query = new GetPendingComplainCount();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet(Name ="GetClosedComplain")]
        public async Task<ActionResult<Response>> GetClosedComplain()
        {
            var query = new GetClosedComplain();
            var result = await _mediator.Send(query);
            return Ok(result);

        }
        [HttpGet(Name = "GetAverageResponseTime")]
        public async Task<ActionResult<Response>> GetAverageResponseTime()
        {
            var query = new GetAverageResponseTime();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = "CloseComplaint")]
        public async Task<ActionResult<Response>> CloseComplaint([FromBody] CloseComplaintQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost(Name = "ForceCloseComplaint")]
        public async Task<ActionResult<Response>> ForceCloseComplaint([FromBody] ForceCloseComplaintQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        ////[RouteIdAuthorize("20")]
        //[HttpPost(Name = "DeleteTerminal")]
        //public async Task<ActionResult<bool>> DeleteTerminal([FromBody] DeleteTerminalCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return result;
        //}
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<TransactionType>>> GetTransactionTypes()
        //{
        //    var command = new GetAllTransactionTypesQuery();
        //    var result = await _mediator.Send(command);
        //    return result.ToList();
        //}


    }
}
