using Authentication.Application.Queries;
using Authentication.Application.Responses;
using Authentication.Core.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Handlers
{
    internal class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserResponseWithSecurity>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public GetUserByUsernameQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserResponseWithSecurity> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var userList = await _userRepository.GetUserByUserame(request.Username);
            return _mapper.Map<UserResponseWithSecurity>(userList);
        }
    }
}
