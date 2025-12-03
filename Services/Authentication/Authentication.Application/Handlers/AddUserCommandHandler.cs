using Authentication.Application.Commands;
using Authentication.Application.Responses;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using AuthenticationManager;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Handlers
{
    internal class AddUserCommandHandler: IRequestHandler<AddUserRequest, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Helper _helper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<AddUserCommandHandler> logger, Helper helper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _helper = helper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            Response response = new Response();         
            try
            {
                var existinguser = await _userRepository.GetUserByEmail(request.Email);
                if (existinguser != null)
                {
                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = $"User Already Exist {request.Email}";
                    response.Data = request;
                    return response;
                }

                var AddUserCommandWithHash = _mapper.Map<AddUserCommandWithHash>(request);
                AddUserCommandWithHash.SecurityKey = Guid.NewGuid().ToString().Replace("-", "");
                AddUserCommandWithHash.PasswordHash = _helper.EncryptString(request.Passowrd, AddUserCommandWithHash.SecurityKey);

                var userToAdd =  _mapper.Map<User>(AddUserCommandWithHash);
                userToAdd.TenantCode=request.TenantCode;
                userToAdd.CreatedBy= _httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value;
                var AddedUser = await _userRepository.AddAsync(userToAdd);
                _logger.LogInformation($"USer {userToAdd} added successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = $"USer {userToAdd.Username} added successfully.";
                response.Data = request;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"User addition failed {ex.Message}.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = $"User addition failed {ex.Message}.";
                return response;
            }
        }
    }
}
