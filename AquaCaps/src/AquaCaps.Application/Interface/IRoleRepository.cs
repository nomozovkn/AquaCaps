using AquaCaps.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Interface;

public interface IRoleRepository
{
    Task<long> InsertRoleAsync(Role role);
    Task<ICollection<User>> GetAllUsersByRoleAsync(string role);
    Task<List<Role>> GetAllRolesAsync();
    Task<long> GetRoleIdAsync(string role);
    Task<Role> GetRoleByIdAsync(long roleId);
    Task UpdateRoleAsync(Role role);
    Task DeleteRoleAsync(Role role);
}
