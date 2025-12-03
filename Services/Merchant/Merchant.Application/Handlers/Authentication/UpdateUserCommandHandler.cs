using Merchants.Application.Commands;
using Merchants.Application.Exceptions;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using AuthenticationManager;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Merchants.Application.Responses;

namespace Merchants.Application.Handlers.Authentication
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response>
    {
        private readonly IMerchant _merchantRepo;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Helper _helper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserCommandHandler> logger, IHttpContextAccessor httpContextAccessor, Helper helper, IMerchant merchantRepo)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _helper = helper;
            _merchantRepo = merchantRepo;
        }
        public async Task<Response> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            Response response = new();
            try
            {
                if (request.UserTypeCode != null && (request.UserTypeCode == 7 || request.UserTypeCode == 8))
                {
                    var merchantZone = await _merchantRepo.GetMerchantByZone(request.ManagementId);
                    var merhcantArea = await _merchantRepo.GetMerchantByArea(request.ManagementId);

                    if (merchantZone != null)
                    {
                        request.MerchantId = merchantZone.ID;
                    }
                    else if (merhcantArea != null)
                    {
                        request.MerchantId = merhcantArea.ID;
                    }
                }

                var userToUpdate = await _userRepository.GetUserById(request.Id);
                if (userToUpdate == null) {
                    _logger.LogError($"User Not found for updation.");
                    throw new UserNotFoundException(nameof(userToUpdate), request.Id);
                }
                var secKey = Guid.NewGuid().ToString().Replace("-", "");
                request.PasswordHash = _helper.EncryptString(request.Password, secKey);
                _mapper.Map(request, userToUpdate, typeof(UpdateUserCommand), typeof(User));
                var userId = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);

                if(userId == null)
                    userToUpdate.UpdatedBy = null;
                else
                {
                    userToUpdate.UpdatedBy = userId;
                    userToUpdate.UpdatedAt = DateTime.Now;
                }


                userToUpdate.SecurityKey = secKey;
                userToUpdate.PasswordHash=request.PasswordHash;
                var UpdatedUser = await _userRepository.UpdateAsync(userToUpdate);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "User Updated Successfully";
                response.Data = null;

                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError($"User updation failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "User Update Failed";
                response.Data = null;

                return response;
            }
        }
    }
}
