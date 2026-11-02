using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace lp.Infrastructure.Data;

public static class AdminSeed
{
   
    public static async Task SeedAdminAsync(LpDbConext conext,IConfiguration configuration)
    {
        bool existingUser=await conext.Users.AnyAsync(u=>u.Role==Domain.Entities.Enum.UserRole.Admin);

        if (existingUser)
        {
           return; 
        }

        var admin=new User
        {
            Name="Admin One",
            Email=configuration["Admin:Email"]!,
            Password=BCrypt.Net.BCrypt.HashPassword(configuration["Admin:Password"]!),
            Role=Domain.Entities.Enum.UserRole.Admin,
            Status=Domain.Entities.Enum.UserStatus.Approved

        };

        conext.Users.Add(admin);
        await conext.SaveChangesAsync();
    }
}