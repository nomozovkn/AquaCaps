using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using AquaCaps.Domain.Entity;

namespace AquaCaps.Application.MapService;

public static class MapUserService
{
    public static User ConvertToUser(UserCreateDto dto, string hashedPassword, string salt)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Password = hashedPassword,
            Salt = salt,
            Address = dto.Address,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            IsActive = true,
            Role = new Role
            {
                Name = "User",
                Description = "Default role for new users"
            }
        };
    }

    public static UserDto ConvertToDto(User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Latitude = user.Latitude,
            Longitude = user.Longitude,
            IsActive = user.IsActive,
            Role = user.Role != null
                ? new RoleDto
                {
                    RoleId = user.Role.RoleId,
                    RoleName = user.Role.Name,
                    Description = user.Role.Description
                }
                : null
        };
    }

    public static User ConvertToEntity(UserDto userDto)
    {
        return new User
        {
            UserId = userDto.UserId,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            UserName = userDto.UserName,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Address = userDto.Address,
            Latitude = userDto.Latitude,
            Longitude = userDto.Longitude,
            IsActive = userDto.IsActive,
            RoleId = userDto.Role?.RoleId ?? 0,
            Role = userDto.Role != null
                ? new Role
                {
                    RoleId = userDto.Role.RoleId,
                    Name = userDto.Role.RoleName,
                    Description = userDto.Role.Description
                }
                : null,

        };
    }
    public static User ConverToUser(UserDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        return new User
        {
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            IsActive = dto.IsActive,
            RoleId = dto.Role?.RoleId ?? 0, // Role bo‘sh bo‘lsa, 0 bo‘ladi — validatsiya qilish kerak
                                            // ⚠ Password, Salt, Orders va RefreshTokens bu yerda set qilinmaydi!
        };
    }
    public static UserDto MapToUserDto(User user)
    {
        if (user == null) return null!;

        return new UserDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            IsActive = user.IsActive,
            Address = user.Address ?? string.Empty,
            Latitude = user.Latitude,
            Longitude = user.Longitude,
            Role = user.Role == null ? null : new RoleDto
            {
                RoleId = user.Role.RoleId,
                RoleName = user.Role.Name ?? string.Empty
            }
        };
    }
    public static AdminDashboardStatsDto MapToAdminDashboardStatsDto(AdminDashboardStats stats)
    {
        return new AdminDashboardStatsDto
        {
            TotalOrders = stats.TotalOrders,
            PendingOrders = stats.PendingOrders,
            CompletedOrders = stats.CompletedOrders,
            CancelledOrders = stats.CancelledOrders,

            TotalClients = stats.TotalClients,
            ActiveClients = stats.ActiveClients,
            InactiveClients = stats.InactiveClients,

            TotalCouriers = stats.TotalCouriers,
            ActiveCouriers = stats.ActiveCouriers,
            InactiveCouriers = stats.InactiveCouriers
        };
    }
    public static RoleDto MapToRoleDto(Role role)
    {
        return new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.Name,
            Description = role.Description

        };
    }

}
