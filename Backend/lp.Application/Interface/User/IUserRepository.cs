using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface IUserRepository
{
    public Task<User?> GetUserByEmail(string email);
    public Task<User?> GetUserById(int id);
    public  Task<List<User>> ViewPendingUsers();
    public Task AddUserAsync(User user);
    public Task SaveChangesAsync();
}