using lp.Application.Interface;
using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace lp.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly LpDbConext _context;
    public UserRepository(LpDbConext context)
    {
        _context=context;
    }
    public async Task<User?> GetUserByEmail(string email)
    {
        return await  _context.Users.FirstOrDefaultAsync(u=>u.Email==email);
    }

    public async Task AddUserAsync(User user)
    {
         await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}