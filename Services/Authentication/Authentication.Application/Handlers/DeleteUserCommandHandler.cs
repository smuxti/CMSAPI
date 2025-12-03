using Authentication.Application.Commands;
using Authentication.Application.Exceptions;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Handlers
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userToDeleted = await _userRepository.GetById(request.Id);
                if (userToDeleted == null)
                {
                    _logger.LogError($"User Not found for updation.");
                    throw new UserNotFoundException(nameof(userToDeleted), request.Id);
                }
                userToDeleted.DeletedBy= _httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value;
                await _userRepository.DeleteAsync(userToDeleted);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"User deletion failed {ex.Message}.");
                return false;
            }
        }
    }
}
