using AutoMapper;
using lp.Application.Dtos;
using lp.Domain.Entities;

namespace lp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User,CreateUserResponse>();
        CreateMap<User,ViewUserDto>();
        CreateMap<Course, CourseResponseDto>();
        CreateMap<Enrollment,ViewEnrollRequestDto>();
    }
}