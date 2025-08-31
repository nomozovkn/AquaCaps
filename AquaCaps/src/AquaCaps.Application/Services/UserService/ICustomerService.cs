using AquaCaps.Application.DTOs;

namespace AquaCaps.Application.Services.UserService;

public interface ICustomerService
{
    Task<long> CreateOrderAsync(OrderCreateDto orderCreateDto,long clientId);
    Task DeleteAsync(long orderId);
    Task<ICollection<OrderDto>> GetAllActiveOrdersAsync(long customerId);
    Task<List<OrderDto>>GetOrdersHistoryAsync(int skip, int take, long customerId);
    Task UpdateAsync(OrderUpdateDto orderUpdateDto);
    
}