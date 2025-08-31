using AquaCaps.Application.DTOs.Admin;
using AquaCaps.Application.Interface;
using AquaCaps.Domain.Entity;

namespace AquaCaps.Application.Services.AdminService;

public class OrderAssignmentService : IOrderAssignmentService
{
    private readonly IOrderAssignmentRepository _orderAssignmentRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly IOrderRepository _orderRepository;
    public OrderAssignmentService(
        IOrderAssignmentRepository orderAssignmentRepository,
        IAdminRepository adminRepository,IOrderRepository orderRepository)
    {
        _orderAssignmentRepository = orderAssignmentRepository;
        _adminRepository = adminRepository;
        _orderRepository = orderRepository;
    }

    public async Task AssignOrderToCourierAsync(long orderId, long courierId)
    {
        bool alreadyAssigned = await _orderAssignmentRepository.IsOrderAssignedAsync(orderId);
        if (alreadyAssigned)
            throw new InvalidOperationException("Buyurtma allaqachon biriktirilgan.");

        var courier = await _adminRepository.GetUserByIdAsync(courierId);
        if (courier == null)
            throw new InvalidOperationException("Kuryer topilmadi.");

        if (!courier.IsActive || courier.Role.Name != "Courier")
            throw new InvalidOperationException("Kuryer faol emas yoki roli noto‘g‘ri.");

        await _orderAssignmentRepository.AssignOrderAsync(orderId, courierId);
    }

    public async Task<OrderAssignmentDto?> GetAssignmentByOrderIdAsync(long orderId)
    {
        var assignment = await _orderAssignmentRepository.GetAssignmentByOrderIdAsync(orderId);
        if (assignment == null)
            return null;

        return MapToDto(assignment);
    }

    public async Task<IEnumerable<OrderAssignmentDto>> GetAssignmentsByCourierIdAsync(long courierId)
    {
        var assignments = await _orderAssignmentRepository.GetAssignmentsByCourierIdAsync(courierId);
        return assignments.Select(MapToDto);
    }
        
    public async Task<bool> IsOrderAssignedAsync(long orderId)
    {
        return await _orderAssignmentRepository.IsOrderAssignedAsync(orderId);
    }

    public async Task UnassignOrderAsync(long orderId)
    {
        bool isAssigned = await _orderAssignmentRepository.IsOrderAssignedAsync(orderId);
        if (!isAssigned)
            throw new InvalidOperationException("Buyurtma biriktirilmagan.");
        var order=await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
        {
            throw new InvalidOperationException("Buyurtma topilmadi.");
        }
        await _orderAssignmentRepository.UnassignOrderAsync(order);
    }

    private OrderAssignmentDto MapToDto(OrderAssignment assignment)
    {
        return new OrderAssignmentDto
        {
            Id = assignment.Id,
            OrderId = assignment.OrderId,
            CourierId = assignment.CourierId,
            CourierName = assignment.Courier.UserName ?? "",
            AssignedAt = assignment.AssignedAt,
            IsActive = assignment.IsActive
        };
    }
}
