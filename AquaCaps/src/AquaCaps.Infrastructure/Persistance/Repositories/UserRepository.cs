using AquaCaps.Application.Interface;
using AquaCaps.Core.Errors;
using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AquaCaps.Infrastructure.Persistance.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<long> InsertAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.UserId;
    }
    public async Task DeleteAsync(long userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateUserRoleAsync(long userId, Role role)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        user.Role = role;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task<ICollection<User>> GetAllCouriersAsync()
    {
        var couriers = await _context.Users
             .Where(u => u.Role.Name == "Courier")
             .ToListAsync();
        return couriers;
    }



    public async Task<ICollection<User>> SelectAllAsync(int skip, int take)
    {
        return await _context.Users
            .Include(u => u.Role)
            .OrderBy(u => u.UserId)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }


    public async Task<User?> SelectByIdAsync(long userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        return user;
    }

    public async Task<User?> SelectByPhoneNumber(string number)
    {
        var user = await _context.Users
             .FirstOrDefaultAsync(u => u.PhoneNumber == number);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        return user;
    }

    public async Task<User?> SelectByUserNameAsync(string userName)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        return user;
    }
    public async Task<List<User>> GetActiveCouriersAsync()
    {
        return await _context.Users
            .Where(u => u.IsActive && u.Role.Name == "Courier")
            .ToListAsync();
    }

    public async Task<int> GetCountUsersAsync()
    {
        var count = await _context.Users.CountAsync();
        return count;
    }
    public async Task<int> CountActiveCouriersAsync()
    {
        return await _context.Users
            .CountAsync(u => u.IsActive && u.Role.Name == "Courier");
    }
    public async Task<User?> GetByPhoneAsync(string phoneNumber)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }
    public async Task<List<User>> GetClientsInactiveSinceAsync(DateTime sinceDate)
    {
        return await _context.Users
            .Where(u => u.Role.Name == "Client" &&  // Faqat mijozlar
                        !_context.Orders.Any(o => o.CreatedByUserId == u.UserId && o.CreatedAt >= sinceDate))
            .ToListAsync();
    }

    public async Task<ICollection<User?>> SelectUsersByRoleAsync(string roleName)
    {
        if(string.IsNullOrEmpty(roleName))
        {
            throw new ArgumentException("Role name cannot be null or empty", nameof(roleName));
        }
        var users = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.Name == roleName)
            .ToListAsync();

        return users;
    }
}
