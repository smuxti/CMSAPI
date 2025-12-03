using MediatR;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{

    public class ManagementHierarchyController : ApiController
    {
        private readonly IMediator _mediator;
        public ManagementHierarchyController(IMediator mediator)
        {
            _mediator = mediator;
        }
        ////[RouteIdAuthorize("2")]
        //[HttpGet(Name = "GetMerchants")]
        //[ProducesResponseType(typeof(IEnumerable<MerchantResponse>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<IEnumerable<MerchantResponse>>> GetMerchants(string name)
        //{
        //    try
        //    {
        //        var query = new GetMerchantListQuery(name);
        //        var merchants = await _mediator.Send(query);
        //        return Ok(merchants);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest("");
        //    }

        //}
        //[RouteIdAuthorize("3")]
        [HttpPost(Name = "AddManagementHierarchy")]
        public async Task<ActionResult<Response>> AddManagementHierarchy([FromBody] AddManagementHierarchyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost(Name = "UpdateManagementHierarchy")]
        public async Task<ActionResult<Response>> UpdateManagementHierarchy([FromBody] UpdateManagementHierarchyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }



        [HttpPost(Name = "DeleteManagementHierarchy")]
        public async Task<ActionResult<Response>> DeleteManagementHierarchy([FromBody] DeleteManagementHierarchyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult> GetManagementHierarchy()
        {
            var command = new GetAllManagementQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> GetManagementHierarchyByID(int id)
        {
            var command = new ManagementHierarcyByIDQuery(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
