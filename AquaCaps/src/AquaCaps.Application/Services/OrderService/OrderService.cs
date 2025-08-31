using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using AquaCaps.Application.Interface;
using AquaCaps.Application.MapService;
using AquaCaps.Core.Errors;
using AquaCaps.Domain.Entity;
using FluentValidation;

namespace AquaCaps.Application.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IOrderAssignmentRepository _orderAssignmentRepository;
    private readonly IValidator<CreateOrderDto> _createOrderValidator;
    private readonly IValidator<OrderUpdateDto> _updateOrderValidator;
    private readonly IValidator<OrderCreateDto> _orderCreateValidator;

    public OrderService(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IOrderAssignmentRepository orderAssignmentRepository,
        IValidator<CreateOrderDto> createOrderValidator,
        IValidator<OrderUpdateDto> updateOrderValidator,
        IValidator<OrderCreateDto> orderCreateValidator)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _orderAssignmentRepository = orderAssignmentRepository;
        _createOrderValidator = createOrderValidator;
        _updateOrderValidator = updateOrderValidator;
        _orderCreateValidator = orderCreateValidator;
    }


    public async Task AutoAssignPendingOrdersAsync()
    {
        var unassignedOrders = await _orderRepository.GetUnassignedOrdersAsync();
        var activeCouriers = await _userRepository.GetActiveCouriersAsync();

        if (!activeCouriers.Any() || !unassignedOrders.Any())
            return;

        int courierCount = activeCouriers.Count();
        int i = 0;

        foreach (var order in unassignedOrders)
        {
            var courier = activeCouriers.ElementAt(i % courierCount);

            var assignment = new OrderAssignment
            {
                OrderId = order.OrderId,
                CourierId = courier.UserId,
                AssignedAt = DateTime.UtcNow
            };

            await _orderAssignmentRepository.AssignOrderAsync(order.OrderId, courier.UserId);
            i++;
        }
    }

    public async Task CancelClientActiveOrderAsync(string clientPhone)
    {
        var client = await _userRepository.GetByPhoneAsync(clientPhone);
        if (client == null)
            throw new EntityNotFoundException($"Client with phone {clientPhone} not found.");

        var activeOrder = await _orderRepository.GetActiveOrderByClientIdAsync(client.UserId);
        if (activeOrder == null)
            throw new InvalidOperationException("Client has no active orders.");

        if (activeOrder.IsDelevered || activeOrder.IsCancelled)
            throw new InvalidOperationException("Client's order is already completed or cancelled.");

        activeOrder.IsCancelled = true;


        await _orderRepository.UpdateAsync(activeOrder);
    }


    public async Task ConfirmDeliveryAsync(long orderId, int returnedCapsules)
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");

        order.IsDelevered = true;
        order.ReturnedCapsuleCount = returnedCapsules;
        order.DeliveryDate = DateOnly.FromDateTime(DateTime.UtcNow);

        await _orderRepository.UpdateAsync(order);
    }

    

    public async Task<long> CreateOrderAsync(OrderCreateDto orderDto)
    {
        var validationResult = await _orderCreateValidator.ValidateAsync(orderDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var order = MapOrderService.MapToOrder(orderDto);
        var id = await _orderRepository.InsertAsync(order);
        return id;
    }

    public async Task DeleteAsync(long orderId)
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");

        await _orderRepository.DeleteAsync(order);
    }

    public async Task<OrderStatsDto> GetAdminOrderStatsAsync()
    {
        var totalOrders = await _orderRepository.GetCountOrdersAsync();
        var assignedOrders = await _orderAssignmentRepository.GetCountOrderAssignmentAsync();
        var unassignedOrders = totalOrders - assignedOrders;
        var completedOrders = await _orderRepository.CountByStatusAsync(true);

        return new OrderStatsDto
        {
            TotalOrders = totalOrders,
            AssignedOrders = assignedOrders,
            UnassignedOrders = unassignedOrders,
            CompletedOrders = completedOrders,
            // Qo‘shimcha statistikani ham qo‘shishingiz mumkin
        };
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync(int skip, int take)
    {
        var orders = await _orderRepository.GetAllAsync(skip, take);
        return orders.Select(o => MapOrderService.MapToOrderDto(o));
    }

    public async Task<OrderDto> GetByIdAsync(long orderId)
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");

        return MapOrderService.MapToOrderDto(order);
    }

    public IQueryable<OrderDto> GetClientOrdersAsync(long clientId)
    {
        var query = _orderRepository.SelectAll()
            .Where(o => o.CreatedByUserId == clientId);
        var orders = query.Select(o => MapOrderService.MapToOrderDto(o));
        return orders;
    }
    public async Task<IEnumerable<UserDto>> GetInactiveClientsOrdersAsync(DateTime sinceDate)
    {
        var clients = await _userRepository.GetClientsInactiveSinceAsync(sinceDate);
        if (clients == null || !clients.Any())
        {
            return new List<UserDto>();
        }
        var clientsDto = clients.Select(c => MapUserService.ConvertToDto(c));
        return clientsDto;
    }
    public async Task MarkAsReturnedAsync(long orderId)  
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");

        order.IsDelevered = false;
        await _orderRepository.UpdateAsync(order);
    }
    public async Task UpdateAsync(OrderUpdateDto orderDto)
    {
        var validationResult = await _updateOrderValidator.ValidateAsync(orderDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var order=MapOrderService.MapToOrder(orderDto);
        var existingOrder = await _orderRepository.SelectByIdAsync(order.OrderId);
        if (existingOrder == null)
        {
            throw new EntityNotFoundException($"Order with ID {order.OrderId} not found.");
        }
        await _orderRepository.UpdateAsync(order);
    }

    //public async Task<bool> IsOrderLockedAsync(long orderId)
    //{
    //    return await _orderAssignmentRepository.IsOrderLockedAsync(orderId);
    //}

    //public async Task LockAssignmentAsync(long orderId)
    //{
    //    await _orderAssignmentRepository.LockOrderAsync(orderId);
    //}
    //public async Task<long> CreateAsync(CreateOrderDto orderDto)
    //{
    //    var validationResult = await _createOrderValidator.ValidateAsync(orderDto);
    //    if (!validationResult.IsValid)
    //        throw new ValidationException(validationResult.Errors);

    //    var order = MapOrderService.MapToOrder(orderDto);
    //    var id = await _orderRepository.CreateAsync(order);
    //    return id;
    //}
    //public async Task<IEnumerable<OrderDto>> GetCourierOrdersAsync(long courierId)
    //{
    //    var orders = await _orderRepository.GetOrdersByCourierIdAsync(courierId);
    //    return orders.Select(o => MapOrderService.MapToOrderDto(o));
    //}
    //public async Task<IEnumerable<OrderDto>> SearchOrdersAsync(string keyword)
    //{
    //    var orders = await _orderRepository.SearchByKeywordAsync(keyword);
    //    return orders.Select(o => MapOrderService.MapToOrderDto(o));
    //}
    //public async Task UnlockAssignmentAsync(long orderId)
    //{
    //    await _orderAssignmentRepository.UnlockOrderAsync(orderId);
    //}
}
