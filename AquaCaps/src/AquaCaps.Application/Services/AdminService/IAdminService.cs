using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Services.AdminService
{
    public interface IAdminService
    {
        Task<List<OrderDto>> GetAllOrdersAsync();                  // Barcha buyurtmalar ro'yxatini olish uchun
        Task<OrderDto> GetOrderByIdAsync(long orderId);             // Muayyan buyurtmani ID bo'yicha olish uchun
        Task<long> CreateOrderAsync(CreateOrderDto orderDto);       // Yangi buyurtma yaratish uchun tel orqali qlingan zakaz
        Task  UpdateOrderAsync(OrderUpdateDto orderDto);       // Mavjud buyurtmani yangilash uchun
        Task DeleteOrderAsync(long orderId);                  // Buyurtmani o'chirish uchun

        Task<bool> AssignOrderToCourierAsync(long orderId, long courierId);  // Buyurtmani ma'lum kuryerga biriktirish uchun
        Task<bool> UnassignCourierAsync(long orderId);                        // Buyurtmani kuryerdan ajratish uchun
        Task<List<UserDto>> GetAvailableCouriersAsync();                      // Bo'sh (faol) kuryerlar ro'yxatini olish uchun

        Task<List<UserDto>> GetAllUsersAsync(int skip ,int take);                      // Barcha foydalanuvchilar ro'yxatini olish uchun
        Task<UserDto> GetUserByIdAsync(long userId);                 // Muayyan foydalanuvchini ID bo'yicha olish uchun
        Task UpdateUserAsync(UserDto userDto);           // Foydalanuvchi ma'lumotlarini yangilash uchun
        Task<bool> DeactivateUserAsync(long userId);                 // Foydalanuvchini faolligini o'chirish (bloklash) uchun

        Task<List<RoleDto>> GetAllRolesAsync();                      // Rol ro'yxatini olish uchun
        Task<List<UserDto>> GetUsersByRoleAsync(string role);

        Task<AdminDashboardStatsDto> GetAdminDashboardStatsAsync();  // Admin boshqaruv paneli uchun umumiy statistikalarni olish
        
        Task<List<OrderDto>> GetOrdersByCourierAsync(long courierId);       // Ma'lum kuryerga biriktirilgan buyurtmalar ro'yxatini olish
        Task AddAdminOrCourier(long userId, long roleId); // Admin yoki kuryer qo'shish uchun
        //Task<List<ClientDto>> GetInactiveClientsAsync(DateTime sinceDate);  // Belgilangan sanadan beri faol bo'lmagan mijozlar ro'yxatini olish
    }

}
