using AutoMapper;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public AdminService(IUserRepository userRepository,IMapper mapper)
    {
        _userRepository=userRepository;
        _mapper=mapper;
    }

    public async Task<List<ViewUserDto>> ViewPendingUsers()
    {
        var users= await _userRepository.ViewPendingUsers();

        return _mapper.Map<List<ViewUserDto>>(users);

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

        return _mapper.Map<ViewUserDto>(user);

    }

    
}