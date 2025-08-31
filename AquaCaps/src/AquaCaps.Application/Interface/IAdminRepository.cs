using AquaCaps.Domain.Entity;

namespace AquaCaps.Application.Interface;

public interface IAdminRepository
{
    Task<List<Order>> GetAllOrdersAsync(int skip, int take);
    Task<Order> GetOrderByIdAsync(long orderId);
    Task<long> CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(long orderId);

    Task<bool> AssignOrderToCourierAsync(long orderId, long courierId);
    Task<bool> UnassignCourierAsync(long orderId);
    Task<List<User>> GetAvailableCouriersAsync();

    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(long userId);
    Task UpdateUserAsync(User user);
    Task<bool> DeactivateUserAsync(long userId);

    Task<List<Role>> GetAllRolesAsync();

    Task<AdminDashboardStats> GetAdminDashboardStatsAsync();

    Task<List<Order>> GetOrdersByCourierAsync(long courierId);
}

