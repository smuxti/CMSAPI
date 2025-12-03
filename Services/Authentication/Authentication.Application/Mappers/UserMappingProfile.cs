using Authentication.Application.Commands;
using Authentication.Application.Responses;
using Authentication.Core.Entities;
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
