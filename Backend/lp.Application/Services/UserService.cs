using lp.Application.Dtos;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class UserService:IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository=userRepository;
    }

    public async Task<CreateUserResponse> RegisterAsync(CreateUserRequest request)
    {
        var existingUser=await _userRepository.GetUserByEmail(request.Email);

        if (existingUser != null)
        {
            throw new Exception("User with that email already exits");
        }

        var user=new User
        {
            Name=request.Name,
            Email=request.Email,
            Password=BCrypt.Net.BCrypt.HashPassword(request.Password),
            Status=Domain.Entities.Enum.UserStatus.Pending,
            
        };

        await _userRepository.AddUserAsync(user);
        await _userRepository.SaveChangesAsync();

        return new CreateUserResponse
        {
            Id=user.Id,
            Name=user.Name,
            Email=user.Email,
            Status=user.Status,
            Role=user.Role
        };

    }

    public async Task<CreateUserResponse> LoginAsync(LoginRequest request)
    {
        var user=await _userRepository.GetUserByEmail(request.Email);

        if (user == null)
        {
            throw new Exception("Cannot find the user");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new Exception("Passwords didnot match");
        }
        
        return new CreateUserResponse
        {
            Id=user.Id,
            Name=user.Name,
            Email=user.Email,
            Status=user.Status,
            Role=user.Role
        };

    }
}