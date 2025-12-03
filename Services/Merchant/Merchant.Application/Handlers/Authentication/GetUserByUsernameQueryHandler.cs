using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merhchants.Application.Queries;

namespace Merchants.Application.Handlers.Authentication
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, Response>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public GetUserByUsernameQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<Response> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            Response baseResponse = new Response();
            var userList = await _userRepository.GetUserByUserame(request.Username);
            baseResponse.Data = userList;
            baseResponse.ResponseDescription = "User Fetched Successfully";
            baseResponse.isSuccess = true;
            baseResponse.ResponseCode = 1;
            return baseResponse;
        }
    }
}
