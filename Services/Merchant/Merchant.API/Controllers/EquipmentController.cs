using MediatR;
using Merchants.Application.Commands.Equipment;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Merchants.API.Controllers
{
    public class EquipmentController : ApiController
    {
        private readonly IMediator _mediator;
        public EquipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> AddEquipment([FromBody] AddEquipmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Response>> UpdateEquipment([FromBody] UpdateEquipmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Response>> DeleteEquipment(int id)
        {
            var command = new DeleteEquipmentById(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult> GetEquipment()
        {
            var command = new GetAllEquipmentQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult> GetEquipmentById(int id)
        {
            var command = new GetEquipmentById(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        

        [HttpGet]
        public async Task<ActionResult> GetEquipmentByCategoryId(int id)
        {
            var command = new GetEquipmentByCategoryId(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
