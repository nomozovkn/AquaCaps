using AquaCaps.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Interface;

public interface ICustomerRepository
{
    Task<long> InsertAsync(Order order);
    Task DeleteAsync(long orderId);
    Task<ICollection<Order>> GetAllOrdersAsync();
    Task UpdateAsync(Order order);
}
