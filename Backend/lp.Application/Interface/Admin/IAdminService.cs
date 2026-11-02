using lp.Application.Dtos;

namespace lp.Application.Interface;

public interface IAdminService
{
    public Task<List<ViewUserDto>> ViewPendingUsers();
    public Task<ViewUserDto> VerifyUserAsync(int userId,VerifyUserDto dto);
}