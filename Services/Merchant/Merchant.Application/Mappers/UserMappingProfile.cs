using Merchants.Application.Commands;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using AuthenticationManager.Models;
using AutoMapper;

namespace Authentication.Application.Mappers
{
    internal class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseWithSecurity>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, AddUserCommandWithHash>().ReverseMap();
            CreateMap<AddUserRequest, AddUserCommandWithHash>().ReverseMap();
            CreateMap<User, UpdateUserCommand>().ReverseMap();
            CreateMap<User, DeleteUserCommand>().ReverseMap();
            CreateMap<User, AuthResponseWithKey>().ReverseMap();
            CreateMap<AuthResponse, AuthResponseWithKey>().ReverseMap();
            CreateMap<AuthResponseWithKey, JwtAuthRequest>().ReverseMap();
        }
    }
}
