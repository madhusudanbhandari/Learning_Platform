using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace lp.Application.Service;

public class UserService:IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository,IConfiguration configuration,IMapper mapper)
    {
        _userRepository=userRepository;
        _configuration=configuration;
        _mapper=mapper;
    }

    public async Task<CreateUserResponse> RegisterAsync(CreateUserRequest request)
    {
        var existingUser=await _userRepository.GetUserByEmail(request.Email);

        if (existingUser != null)
        {
            throw new BadRequestException("User with that email already exits");
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

        return _mapper.Map<CreateUserResponse>(user);

    }

    public async Task<CreateUserResponse> LoginAsync(LoginRequest request)
    {
        var user=await _userRepository.GetUserByEmail(request.Email);

        if (user == null)
        {
            throw new NotFoundException("Cannot find the user");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new BadRequestException("Passwords didnot match");
        }
        
        return new CreateUserResponse
        {
            Id=user.Id,
            Name=user.Name,
            Email=user.Email,
            Status=user.Status,
            Role=user.Role,
            Token=CreateJwtToken(user)
        };

    }

    private string CreateJwtToken(User user)
    {
        var key=_configuration["Jwt:Key"]!;

        var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials=new SigningCredentials(securityKey, algorithm:SecurityAlgorithms.HmacSha256);


        var claims=new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())

        };

        var token=new JwtSecurityToken(
            issuer:_configuration["Jwt:Issuer"],
            audience:_configuration["Jwt:Audience"],
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials:credentials           


        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
