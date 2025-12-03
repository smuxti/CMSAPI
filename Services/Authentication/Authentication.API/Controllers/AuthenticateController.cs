using Authentication.Application.Commands;
using Authentication.Application.Queries;
using Authentication.Application.Responses;
using Authentication.Core.Entities;
using AuthenticationManager;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    public class AuthenticateController : ApiController
    {
        private readonly IMediator _mediator;

        //private readonly IConnectionMultiplexer _redis;
        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Auth([FromBody] AuthRequest command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Response>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        //[Authorize]
        //[RouteIdAuthorize("1")]
        [HttpPost]
        public async Task<ActionResult<Response>> AddUser([FromBody] AddUserRequest command)
        {
            var a = await _mediator.Send(command);
            return Ok(a);
        }
        [HttpPost]
        public async Task<ActionResult<Response>> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var a = await _mediator.Send(command);
            return Ok(a);
        }
        [HttpPost]
        public async Task<ActionResult<Response>> DeleteUser([FromBody] DeleteUserCommand command)
        {
            var a = await _mediator.Send(command);
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<User>> GetUserByMerchantId([FromBody] GetAllUserByMerchantIdCommand command)
        {
            var a = await _mediator.Send(command);
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<User>> GetUserById([FromBody] GetUserByIdCommand command)
        {
            var a = await _mediator.Send(command);
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<User>> GetUserByEmail([FromBody] GetUserByEmailQuery command)
        {
            var a = await _mediator.Send(command);
            return Ok(a);
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<User>> GetUsers()
        {
            var command = new GetAllUserCommand();
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpGet("Test")]
        public async Task<ActionResult<string>> Test(string Email)
        {
            var command = new TestEmailQuery(Email);
            var result = await _mediator.Send(command);
            return Ok(result);

        }

        [HttpGet("GetUserTypes")]
        public async Task<ActionResult<UserType>> GetUserTypes()
        {
            var command = new GetUserTypesCommands();
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("UserResourceUpload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UserResource>> UserResourceUpload([FromForm] AddResourceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("GetAllByMerchantId")]
        public async Task<ActionResult<UserResource>> UserResourceUpload([FromBody]  GetAllResourcesByMerchIdCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("SetOTP")]
        public async Task<ActionResult<UserResource>> SetOTP([FromBody] SetForgotPasswordOTPCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("GetOTP")]
        public async Task<ActionResult<UserResource>> GetOTP([FromBody] GetForgotPasswordOTPQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

        }
    }
}