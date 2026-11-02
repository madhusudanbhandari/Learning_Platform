
using lp.Application.Dtos;
using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface IUserService
{
    Task<CreateUserResponse> RegisterAsync(CreateUserRequest request);
    Task<CreateUserResponse> LoginAsync(LoginRequest login);
}