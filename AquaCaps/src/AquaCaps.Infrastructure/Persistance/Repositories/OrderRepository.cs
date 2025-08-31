using AquaCaps.Application.Interface;
using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AquaCaps.Infrastructure.Persistance.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<long> InsertAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order.OrderId;
    }
    public async Task DeleteAsync(Order order)
    {
        var existingOrder = await _context.Orders.FindAsync(order.OrderId);
        _context.Orders.Remove(existingOrder);
        await _context.SaveChangesAsync();
    }

    public IQueryable<Order> SelectAll()
    {
        var orders = _context.Orders
            .Include(o => o.CreatedByUser)
            .Include(o => o.AssignedCourier)
            .Include(o => o.OrderAssignments);
        return orders;
    }

    public async Task<ICollection<Order>> GetAllAsync(int skip, int take)
    {
        var orders = await _context.Orders
            .Include(o => o.CreatedByUser)
            .Include(o => o.AssignedCourier)
            .Include(o => o.OrderAssignments)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return orders;
    }

    public async Task<Order?> SelectByIdAsync(long orderId)
    {
        return await _context.Orders
            .Include(o => o.AssignedCourier)
            .Include(o => o.OrderAssignments)
            .Include(o => o.CreatedByUser)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }


    public async Task UpdateAsync(Order order) // admin uchun update
    {

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

    }

    public async Task UpdateOrderByClientAsync(Order order) // client uchun update
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
    public async Task<int> GetCountOrdersAsync()
    {
        return await _context.Orders.CountAsync();
    }

    public async Task<int> GetCountUnassingnAsync()
    {
        return await _context.Orders.CountAsync(o => o.AssignedCourierId == null);
    }

    public Task<List<Order>> GetOrdersByCourierAsync(long courierId)
    {
        return _context.Orders
            .Where(o => o.AssignedCourierId == courierId)
            .Include(o => o.CreatedByUser)
            .Include(o => o.AssignedCourier)
            .Include(o => o.OrderAssignments)
            .ToListAsync();
    }
    public async Task<List<Order>> GetUnassignedOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.AssignedCourier)
            .Where(o => o.AssignedCourier == null)
            .ToListAsync();
    }
    public async Task<Order?> GetActiveOrderByClientIdAsync(long clientId)
    {
        return await _context.Orders
            .Include(o => o.AssignedCourier)
            .Include(o => o.CreatedByUser)
            .Where(o => o.CreatedByUserId == clientId && o.IsDelevered==false) // yoki IsDelivered == false
            .OrderByDescending(o => o.CreatedAt) // oxirgi buyurtmasini olish uchun
            .FirstOrDefaultAsync();
    }

    public async Task<int> CountByStatusAsync(bool isDelivered)
    {
        return await _context.Orders
            .Where(o => o.IsDelevered == isDelivered)
            .CountAsync();
    }

}
