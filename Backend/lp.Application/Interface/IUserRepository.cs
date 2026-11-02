using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface IUserRepository
{
    public Task<User?> GetUserByEmail(string email);
    public Task AddUserAsync(User user);
    public Task SaveChangesAsync();
}