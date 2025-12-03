using Authentication.Application.Commands;
using Authentication.Application.Exceptions;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using AuthenticationManager;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Handlers
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Helper _helper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserCommandHandler> logger, IHttpContextAccessor httpContextAccessor,Helper helper)
        {
             _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _helper = helper;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userToUpdate = await _userRepository.GetById(request.Id);
                if (userToUpdate == null) {
                    _logger.LogError($"User Not found for updation.");
                    throw new UserNotFoundException(nameof(userToUpdate), request.Id);
                }
                var secKey = Guid.NewGuid().ToString().Replace("-", "");
                request.PasswordHash = _helper.EncryptString(request.Password, secKey);
                _mapper.Map(request, userToUpdate, typeof(UpdateUserCommand), typeof(User));
                userToUpdate.UpdatedBy= _httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value;
                userToUpdate.SecurityKey = secKey;
                userToUpdate.PasswordHash=request.PasswordHash;
                var UpdatedUser = await _userRepository.UpdateAsync(userToUpdate);
                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError($"User updation failed {ex.Message}.");
                return false;
            }
        }
    }
}
