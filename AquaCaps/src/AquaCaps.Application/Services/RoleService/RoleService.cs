using AquaCaps.Application.DTOs;
using AquaCaps.Application.Interface;
using AquaCaps.Core.Errors;

namespace AquaCaps.Application.Services.RoleService;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public async Task DeleteRoleAsync(long roleId)
    {
        var role=await _roleRepository.GetRoleByIdAsync(roleId);
        if (role == null)
        {
            throw new EntityNotFoundException($"Role with ID {roleId} not found.");
        }
        await _roleRepository.DeleteRoleAsync(role);

    }

    public async Task<ICollection<RoleDto>> GetAllRolesAsync()
    {
        var roles=await _roleRepository.GetAllRolesAsync();
        if (roles == null || !roles.Any())
        {
            return new List<RoleDto>();
        }
        var roleDtos= roles.Select(role => new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.Name,
            Description = role.Description
        }).ToList();
        return roleDtos;
    }

   
    public async Task UpdateRoleAsync(long roleId, string newRoleName)
    {
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        if (role == null)
        {
            throw new EntityNotFoundException($"Role with ID {roleId} not found.");
        }
        role.Name = newRoleName;
        await _roleRepository.UpdateRoleAsync(role);

    }

    public async Task<long> AddRoleAsync(RoleDto role)
    {
        var roleEntity = new Domain.Entity.Role
        {
            Name = role.RoleName,
            Description = role.Description
        };
        await _roleRepository.InsertRoleAsync(roleEntity);
        return roleEntity.RoleId;
    }
    

}
