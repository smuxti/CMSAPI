using Merchants.Application.Commands;
using Merchants.Application.Exceptions;
using Merchants.Core.Entities ;
using Merchants.Core.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Authentication
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<DeleteUserCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
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
                var userToDeleted = await _userRepository.GetUserById(request.Id);
                if (userToDeleted == null)
                {
                    _logger.LogError($"User Not found for updation.");
                    throw new UserNotFoundException(nameof(userToDeleted), request.Id);
                }
                userToDeleted.DeletedBy= Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
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
