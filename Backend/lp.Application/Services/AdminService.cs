using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    public AdminService(IUserRepository userRepository)
    {
        _userRepository=userRepository;
    }

    public async Task<List<ViewUserDto>> ViewPendingUsers()
    {
        var users= await _userRepository.ViewPendingUsers();

        return users.Select(user=>new ViewUserDto
        {
            Id=user.Id,
            Name=user.Name,
            Email=user.Email,
            Role=user.Role,
            Status=user.Status
        }).ToList();

    }

    public async Task<ViewUserDto> VerifyUserAsync(int id, VerifyUserDto dto)
    {
        var user=await _userRepository.GetUserById(id);

        if (user == null)
        {
            throw new NotFoundException("Cant find the user");
        }

        user.Status=dto.Status;
        user.Role=dto.Role;

        await _userRepository.SaveChangesAsync();

        return new ViewUserDto
        {
            Id=user.Id,
            Name=user.Name,
            Email=user.Email,
            Role=user.Role,
            Status=user.Status
        };

    }

    
}