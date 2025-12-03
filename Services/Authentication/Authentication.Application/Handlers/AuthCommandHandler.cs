using Authentication.Application.Commands;
using Authentication.Application.Responses;
using Authentication.Core.Interfaces;
using AuthenticationManager;
using AuthenticationManager.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Authentication.Application.Handlers
{
    internal class AuthCommandHandler : IRequestHandler<AuthRequest, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly Helper _helper;
        private readonly JwtTokenHandler _jwtTokenHandler;
        public AuthCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserCommandHandler> logger, JwtTokenHandler jWTExtention, Helper helper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _helper = helper;
            _jwtTokenHandler = jWTExtention;//Token
        }
        public async Task<Response> Handle(AuthRequest request, CancellationToken cancellationToken)
        {
            Response response =new Response();
            var user = await _userRepository.GetUserByUserame(request.username);
            if (user == null)
            {
                _logger.LogError($"User Not found.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "User not found.";
                return response;
            }
            var role = await _userRepository.GetRoleByTypeId(user.UserTypeCode);
            var routes = await _userRepository.GetRoutesByRoleTypeId(role.TypeCode);
            
            if (!_helper.ValidatePassword(request.password, user.PasswordHash, user.SecurityKey))
            {
                _logger.LogError($"Invalid username password.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Invalid username password.";
                return response;
            };
            
            AuthResponseWithKey authResponseWithKey = _mapper.Map<AuthResponseWithKey>(user);
            AuthResponse authResponse = _mapper.Map<AuthResponse>(authResponseWithKey);
            authResponse.Id = user.Id;
            authResponse.Role = role.TypeName;
            if (user != null)
            {
                JwtAuthRequest authRequest = _mapper.Map<JwtAuthRequest>(authResponseWithKey);
                authRequest.Role = role.TypeName;
                authRequest.RoleTypeCode = role.TypeCode;
                authRequest.TenantCode = user.TenantCode;
                (var jwt, string refreshToken,DateTime refreshTokenExpiry) = await _jwtTokenHandler.GenerateToken(authRequest,routes.ToList());

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiry;

                await _userRepository.UpdateAsync(user);
                _logger.LogInformation($"User {user.Username} access granted.");
                authResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
                authResponse.RefreshToken = refreshToken;
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "User access granted.";
                response.Data = authResponse;
            }

            return response;

        }
    }
}
