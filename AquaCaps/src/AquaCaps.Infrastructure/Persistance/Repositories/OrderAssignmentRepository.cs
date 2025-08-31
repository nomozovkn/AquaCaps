using AquaCaps.Application.Interface;
using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AquaCaps.Infrastructure.Persistance.Repositories;

public class OrderAssignmentRepository : IOrderAssignmentRepository
{
    private readonly AppDbContext _context;
    public OrderAssignmentRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task AssignOrderAsync(long orderId, long courierId)
    {
        // 1. Avval bu buyurtmaga boshqa biriktirish mavjudligini tekshir
        var existingAssignment = await _context.OrderAssignments
            .FirstOrDefaultAsync(x => x.OrderId == orderId);

        if (existingAssignment != null)
        {
            throw new InvalidOperationException($"Order {orderId} is already assigned.");
        }

        // 2. Yangi biriktirish yaratish
        var assignment = new OrderAssignment
        {
            OrderId = orderId,
            CourierId = courierId,
            AssignedAt = DateTime.UtcNow
        };

        _context.OrderAssignments.Add(assignment);
        await _context.SaveChangesAsync();
    }



    public async Task UnassignOrderAsync(Order order) // buyurtmani kuryerdan bo'shatish
    {
        // 1. Orders jadvalidagi AssignedCourierId ni null qilish
        order.AssignedCourierId = null;
        _context.Orders.Update(order);

        // 2. OrderAssignments jadvalidan ham o'chirish kerak bo'lishi mumkin
        var assignment = await _context.OrderAssignments.FirstOrDefaultAsync(a => a.OrderId == order.OrderId);
        if (assignment != null)
        {
            _context.OrderAssignments.Remove(assignment);
        }

        await _context.SaveChangesAsync();

    }

    public async Task<OrderAssignment?> GetAssignmentByOrderIdAsync(long orderId) // buyurtmaga tayinlangan kuryerni olish
    {
        return await _context.OrderAssignments
            .Include(a => a.Courier) // optional
            .FirstOrDefaultAsync(a => a.OrderId == orderId);
    }

    public async Task<IEnumerable<OrderAssignment>> GetAssignmentsByCourierIdAsync(long courierId) // kuryerga biriktirilgan buyurtmalarni olish
    {
        return await _context.OrderAssignments
            .Where(a => a.CourierId == courierId)
            .Include(a => a.Order) // optional
            .ToListAsync();
    }

    public async Task<bool> IsOrderAssignedAsync(long orderId)// buyurtma tayinlanganligini tekshirish
    {
        return await _context.OrderAssignments
            .AnyAsync(a => a.OrderId == orderId);
    }
    public async Task<List<long>> GetCurrentlyAssignedCourierIdsAsync()
    {
        return await _context.OrderAssignments
            .Select(a => a.CourierId)
            .Distinct()
            .ToListAsync();
    }
    public async Task<int> GetCountOrderAssignmentAsync()
    {
        return await _context.OrderAssignments.CountAsync();
    }


}


