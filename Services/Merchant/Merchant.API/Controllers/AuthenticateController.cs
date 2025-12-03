using Merchants.Application.Commands;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using AuthenticationManager;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication.Application.Queries;
using Authentication.Application.Commands;

namespace Merchants.API.Controllers
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

        #region Routes

        [HttpGet("GetRoutes")]
        public async Task<ActionResult<Response>> GetRoutes()
        {
            var command = new GetAllRoutesCommandQuery();
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("DeleteRoutes")]
        public async Task<ActionResult<Response>> DeleteRoutes([FromQuery] int Id)
        {
            var command = new DeleteRoutesQuery(Id);
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("AddRoute")]
        public async Task<ActionResult<Response>> AddRoute([FromBody] AddRouteCommand model)
        {
            var result = await _mediator.Send(model);
            return Ok(result);

        }
        [HttpPost("UpdateRoute")]
        public async Task<ActionResult<Response>> UpdateRoute([FromBody] UpdateRouteCommand model)
        {
            var result = await _mediator.Send(model);
            return Ok(result);

        }

        [HttpPost("AddRoleRoute")]
        public async Task<ActionResult<Response>> AddRoleRoute([FromBody] AddRoleRoutesCommand model)
        {
            var result = await _mediator.Send(model);
            return Ok(result);

        }
        [HttpPost("DeleteRoleRoute")]
        public async Task<ActionResult<Response>> DeleteRoleRoute([FromBody] DeleteRoleRoutsCommand model)
        {
            var result = await _mediator.Send(model);
            return Ok(result);

        }
        [HttpGet("GetRoleRoutesByUserType")]
        public async Task<ActionResult<Response>> GetRoleRoutesByUserType([FromQuery] int id)
        {
            var command = new GetRoutesByUserTypeQuery(id);
            var result = await _mediator.Send(command);
            return Ok(result);

        }


        #endregion Routes

        #region  UserType
        [HttpGet("GetUserType")]
        public async Task<ActionResult<Response>> GetUserType()
        {
            var command = new GetAllUserTypeQuery();
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("DeleteUserType")]
        public async Task<ActionResult<Response>> DeleteUserType([FromBody] DeleteUserTypeQuery command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

        }
        [HttpPost("AddUserType")]
        public async Task<ActionResult<Response>> AddUserType([FromBody] AddUserTypeCommand model)
        {
            var result = await _mediator.Send(model);
            return Ok(result);
        }
        [HttpPost("UpdateUserType")]
        public async Task<ActionResult<Response>> UpdateUserType([FromBody] UpdateUserTypeCommand model)
        {
            var result = await _mediator.Send(model);
            return Ok(result);
        }



        #endregion UserType
    }
}