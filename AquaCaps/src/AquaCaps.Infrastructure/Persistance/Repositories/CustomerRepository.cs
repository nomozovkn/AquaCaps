using AquaCaps.Application.Interface;
using AquaCaps.Core.Errors;
using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AquaCaps.Infrastructure.Persistance.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<long> InsertAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order.OrderId;
    }
    public async Task DeleteAsync(long orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException("Order not found");
        }
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

    }

    public async Task<ICollection<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
              .Include(o => o.CreatedByUser)
              .Include(o => o.AssignedCourier)
              .ToListAsync();

    }

    public async Task UpdateAsync(Order order)
    {
        var existingOrder = await _context.Orders.FindAsync(order.OrderId);

        if (existingOrder == null)
            throw new EntityNotFoundException("Order not found");


        _context.Entry(existingOrder).CurrentValues.SetValues(order);

        await _context.SaveChangesAsync();
    }

}
