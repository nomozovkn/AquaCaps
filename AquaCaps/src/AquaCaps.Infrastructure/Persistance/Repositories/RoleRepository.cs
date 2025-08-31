using AquaCaps.Application.Interface;
using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AquaCaps.Infrastructure.Persistance.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        var roles = await _context.Roles.ToListAsync();
        return roles;
    }

    public async Task<ICollection<User>> GetAllUsersByRoleAsync(string role)
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.Name == role)
            .ToListAsync();
        return users;
    }

    public async Task<long> GetRoleIdAsync(string role)
    {
        var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role);
        if (roleEntity == null)
        {
            throw new KeyNotFoundException($"Role '{role}' not found.");
        }
        return roleEntity.RoleId;
    }
    public async Task<Role> GetRoleByIdAsync(long roleId)
    {
        var role = await _context.Roles.FindAsync(roleId);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");
        }
        return role;
    }

    public async Task<long> InsertRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return role.RoleId;

    }

    public Task UpdateRoleAsync(Role role)
    {
        var existingRole = _context.Roles.Find(role.RoleId);
        if (existingRole == null)
        {
            throw new KeyNotFoundException($"Role with ID {role.RoleId} not found.");
        }
        existingRole.Name = role.Name;
        existingRole.Description = role.Description;
        _context.Roles.Update(existingRole);
        return _context.SaveChangesAsync();
    }
    public async Task DeleteRoleAsync(Role role)
    {
        var existingRole = await _context.Roles.FindAsync(role.RoleId);
        if (existingRole == null)
        {
            throw new KeyNotFoundException($"Role with ID {role.RoleId} not found.");
        }
        _context.Roles.Remove(existingRole);
        await _context.SaveChangesAsync();
    }
}
