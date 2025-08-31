using AquaCaps.Application.DTOs;
using AquaCaps.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Interface;

public interface IOrderRepository
{
    Task<long> InsertAsync(Order order);
    Task DeleteAsync(Order order);
    Task<Order?> SelectByIdAsync(long OrderId);
    Task<ICollection<Order>> GetAllAsync(int skip, int take);
   
    IQueryable<Order> SelectAll();
    Task UpdateAsync(Order order);
    Task UpdateOrderByClientAsync(Order order);
    public Task<int> GetCountOrdersAsync();
    public Task<int> GetCountUnassingnAsync();
    public Task<List<Order>> GetOrdersByCourierAsync(long courierId);
    Task<List<Order>> GetUnassignedOrdersAsync();
    Task<Order?> GetActiveOrderByClientIdAsync(long clientId);
    Task<int> CountByStatusAsync(bool isDelivered);

}
