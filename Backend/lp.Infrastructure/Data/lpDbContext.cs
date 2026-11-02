using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace lp.Infrastructure;

public class LpDbConext : DbContext
{
    public LpDbConext(DbContextOptions<LpDbConext> options) : base(options)
    {
        
    }

    public DbSet<User> Users=>Set<User>();
}