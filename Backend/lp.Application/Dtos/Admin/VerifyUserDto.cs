using lp.Domain.Entities.Enum;

namespace lp.Application.Dtos;


public class VerifyUserDto
{
    public UserStatus Status{get;set;}
    public UserRole Role{get;set;}
}