using Merchants.Application.Commands;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using AuthenticationManager;
using AuthenticationManager.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Authentication
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;
        private readonly JwtTokenHandler _jwtTokenHandler;
        private readonly IMapper _mapper;

        public RefreshTokenCommandHandler(IUserRepository repository, ILogger<RefreshTokenCommandHandler> logger, JwtTokenHandler jwtTokenHandler,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _jwtTokenHandler = jwtTokenHandler;
            _mapper = mapper;
        }

        public async Task<Response> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            var user = await _repository.GetUserByRefreshToken(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Invalid or expired refresh token.";
                return response;
            }


            var role = await _repository.GetRoleByTypeId(user.UserTypeCode);
            var roleRoutes = await _repository.GetRoutesByRoleTypeId(role.TypeCode);

            AuthResponseWithKey authResponseWithKey = _mapper.Map<AuthResponseWithKey>(user);
            AuthResponse authResponse = _mapper.Map<AuthResponse>(authResponseWithKey);
            authResponse.Id = user.Id;
            authResponse.Role = role.TypeName;

            JwtAuthRequest authRequest = _mapper.Map<JwtAuthRequest>(authResponseWithKey);
            authRequest.Role = role.TypeName;
            authRequest.RoleTypeCode = role.TypeCode;


            (var jwt, var newRefreshToken,DateTime refreshTokenExpiry) = await _jwtTokenHandler.GenerateToken(authRequest, roleRoutes.ToList());

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiry;
            await _repository.UpdateAsync(user);

            response.isSuccess = true;
            response.ResponseCode = 1;
            response.ResponseDescription = "Token refreshed successfully.";
            response.Data = new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken = newRefreshToken
            };

            return response;


        }
    }
}
