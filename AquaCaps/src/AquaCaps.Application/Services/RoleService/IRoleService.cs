using AquaCaps.Application.DTOs;

namespace AquaCaps.Application.Services.RoleService;

public interface IRoleService
{
    Task<long> AddRoleAsync(RoleDto role); // Yangi rol qo'shish
    Task<ICollection<RoleDto>> GetAllRolesAsync(); // Barcha rollarni olish
    Task DeleteRoleAsync(long roleId); // Rolni o'chirish
    Task UpdateRoleAsync(long roleId, string newRoleName); // Rolni yangilash
}