using lp.Domain.Entities.Enum;

namespace lp.Domain.Entities;

public class User
{
    public int Id{get;set;}
    public string Name{get;set;}=string.Empty;
    public string Email{get;set;}=string.Empty;
    public string Password{get;set;}=string.Empty;
    public UserRole Role{get;set;}
    public UserStatus Status{get;set;}

}