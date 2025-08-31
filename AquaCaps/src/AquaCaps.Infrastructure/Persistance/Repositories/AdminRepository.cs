using AquaCaps.Application.Interface;
using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AquaCaps.Infrastructure.Persistance.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;
    public AdminRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> AssignOrderToCourierAsync(long orderId, long courierId) // Buyurtmani kuryerga biriktirish
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            return false; // Buyurtma topilmadi

        var courier = await _context.Users.FindAsync(courierId);
        if (courier == null || !courier.IsActive)
            return false; // Kuryer topilmadi yoki faol emas

        order.AssignedCourierId = courierId;
        order.AssignedAt = DateTime.UtcNow;

        // Optional: OrderAssignment jadvaliga tarix uchun yozuv qo'shish
        var orderAssignment = new OrderAssignment
        {
            OrderId = orderId,
            CourierId = courierId,
            AssignedAt = DateTime.UtcNow,
            IsActive = true
        };
        await _context.OrderAssignments.AddAsync(orderAssignment);

        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<long> CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order.OrderId;
    }

    public async Task<bool> DeactivateUserAsync(long userId) // Foydalanuvchini bloklash
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;  // Foydalanuvchi topilmadi

        if (!user.IsActive)
            return true;   // Allaqachon faol emas

        user.IsActive = false;
        await _context.SaveChangesAsync();
        return true; // Muvaffaqiyatli bloklandi
    }


    public async Task DeleteOrderAsync(long orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<AdminDashboardStats> GetAdminDashboardStatsAsync() // Admin boshqaruv paneli uchun statistikalar
    {
        var totalOrders = await _context.Orders.CountAsync();

        // Faol kuryerlar — IsActive = true va Role nomi "Courier" bo‘lganlar
        var activeCouriers = await _context.Users
            .Include(u => u.Role)
            .CountAsync(u => u.IsActive && u.Role.Name == "Courier");

        var totalUsers = await _context.Users.CountAsync();

        // Faol bo‘lmagan foydalanuvchilar (IsActive = false)
        var inactiveUsers = await _context.Users.CountAsync(u => !u.IsActive);

        return new AdminDashboardStats
        {
            TotalOrders = totalOrders,
            ActiveCouriers = activeCouriers,
            TotalClients = totalUsers,
            InactiveClients = inactiveUsers
        };
    }



    public async Task<List<Order>> GetAllOrdersAsync(int skip, int take)
    {
        if (skip < 0 || take <= 0)
        {
            throw new ArgumentOutOfRangeException("Invalid pagination parameters.");
        }
        var orders = await _context.Orders
            .Include(o => o.CreatedByUser)
            .Include(o => o.AssignedCourier)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return orders;
    }

    public async Task<List<Role>> GetAllRolesAsync() => await _context.Roles.ToListAsync();
    public async Task<List<User>> GetAllUsersAsync() => await _context.Users.Include(u => u.Role).ToListAsync();

    public async Task<List<User>> GetAvailableCouriersAsync() // Faol kuryerlar ro'yxatini olish
    {

        var courier= await _context.Users
            .Include(u => u.Role)
            .Where(u => u.IsActive && u.Role.Name == "Courier")
            .ToListAsync();
        return courier;
    }


    public async Task<Order> GetOrderByIdAsync(long orderId)
    {
        var order = await _context.Orders
            .Include(o => o.CreatedByUser)
            .Include(o => o.AssignedCourier)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        return order;
    }


    public async Task<List<Order>> GetOrdersByCourierAsync(long courierId) // kurierga briktirilgan orderlar
    {
        var orders = await _context.Orders
            .Where(o => o.AssignedCourierId == courierId)
            .Include(o => o.CreatedByUser)      // Buyurtmani kim yaratgan
            .Include(o => o.AssignedCourier)    // Kuryer haqida ma'lumot
            .ToListAsync();

        return orders;
    }


    public Task<User> GetUserByIdAsync(long userId)
    {
        var user = _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return user;
    }

    public async Task<bool> UnassignCourierAsync(long orderId) // Buyurtmani kuryerdan ajratish
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
            return false;

        order.AssignedCourierId = null;
        order.AssignedCourier = null; // ixtiyoriy, agar eager loading bo‘lsa

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
