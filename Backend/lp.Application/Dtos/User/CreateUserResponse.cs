using lp.Domain.Entities.Enum;
namespace lp.Application.Dtos;
public class CreateUserResponse
{
    public int Id{get;set;}
    public string Name{get;set;}=string.Empty;
    public string Email{get;set;}=string.Empty;
    public UserRole Role{get;set;}
    public UserStatus Status{get;set;}
}