using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using AquaCaps.Application.Interface;
using AquaCaps.Application.MapService;
using AquaCaps.Core.Errors;
using AquaCaps.Domain.Entity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AquaCaps.Application.Services.AdminService;

public class AdminService : IAdminService
{

    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IOrderAssignmentRepository _orderAssignmentRepository;
    private readonly IValidator<CreateOrderDto> _createOrderValidator;
    private readonly IValidator<OrderUpdateDto> _orderUpdateValidator;


    public AdminService(IUserRepository userRepository, IOrderRepository orderRepository, IRoleRepository roleRepository, IOrderAssignmentRepository orderAssignmentRepository, IValidator<CreateOrderDto> createOrderDtoValidator, IValidator<OrderUpdateDto> orderUpdateDto)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _roleRepository = roleRepository;
        _orderAssignmentRepository = orderAssignmentRepository;
        _createOrderValidator = createOrderDtoValidator;
        _orderUpdateValidator = orderUpdateDto;

    }
    public async Task AddAdminOrCourier(long userId, long roleId)
    {
        var user = await _userRepository.SelectByIdAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with ID {userId} not found.");
        }
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        if (role == null)
        {
            throw new EntityNotFoundException($"Role with ID {roleId} not found.");
        }
        await _userRepository.UpdateUserRoleAsync(userId, role);
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.SelectAll().ToListAsync();

        var orderDtos = orders.Select(o => MapOrderService.MapToOrderDto(o)).ToList();

        return orderDtos;
    }


    public async Task<OrderDto> GetOrderByIdAsync(long orderId)
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");
        }
        var orderDto = MapOrderService.MapToOrderDto(order);
        return orderDto;

    }

    public Task<long> CreateOrderAsync(CreateOrderDto orderDto)                                                                                // not implement
    {

        throw new NotImplementedException();

    }

    public async Task UpdateOrderAsync(OrderUpdateDto orderDto)
    {
        var validationResult = _orderUpdateValidator.Validate(orderDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // 1. Bazadan mavjud orderni topish
        var existingOrder = await _orderRepository.SelectByIdAsync(orderDto.OrderId);
        if (existingOrder is null)
        {
            throw new NotFoundException($"Order with ID {orderDto.OrderId} not found");
        }

        // 2. Property'larni update qilish
        existingOrder.Address = orderDto.Address;
        existingOrder.OrderedCapsuleCount = orderDto.OrderedCapsuleCount;
        existingOrder.ReturnedCapsuleCount = orderDto.ReturnedCapsuleCount;
        existingOrder.DeliveryDate = orderDto.DeliveryDate;
        existingOrder.Note = orderDto.Note;

        // 3. Yangilash
        await _orderRepository.UpdateAsync(existingOrder);
    }


    public async Task DeleteOrderAsync(long orderId)
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");
        }
        await _orderRepository.DeleteAsync(order);
    }

    public async Task<bool> AssignOrderToCourierAsync(long orderId, long courierId)
    {
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");
        }

        var courier = await _userRepository.SelectByIdAsync(courierId);
        if (courier == null)
        {
            throw new EntityNotFoundException($"Courier with ID {courierId} not found.");
        }

        // Buyurtma allaqachon biriktirilganmi?
        var isAlreadyAssigned = await _orderAssignmentRepository.IsOrderAssignedAsync(orderId);
        if (isAlreadyAssigned)
        {
            throw new InvalidOperationException($"Order with ID {orderId} is already assigned.");
        }
        // Bazaga saqlaymiz
        await _orderAssignmentRepository.AssignOrderAsync(orderId,courierId);

        return true;
    }


    public async Task<bool> UnassignCourierAsync(long orderId)
    {
        // 1. Avval buyurtmaga kuryer biriktirilganmi yoki yo‘qmi tekshiramiz
        var isAssigned = await _orderAssignmentRepository.IsOrderAssignedAsync(orderId);
        if (!isAssigned)
            return false; // Tayinlanmagan bo‘lsa, false qaytaramiz

        // 2. Ajratishni amalga oshiramiz
        var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");
        }
        await _orderAssignmentRepository.UnassignOrderAsync(order);

        // 3. (Ixtiyoriy) Logger, bildirishnoma, tarixga yozish va hokazolar shu yerda bo'lishi mumkin

        return true;
    }


    public async Task<List<UserDto>> GetAvailableCouriersAsync()
    {
        // 1. Faol kuryerlar ro'yxatini olish
        var activeCouriers = await _userRepository.GetActiveCouriersAsync();

        // 2. Hozirda buyurtmaga tayinlanmagan kuryerlar Id larini olish
        var busyCourierIds = await _orderAssignmentRepository.GetCurrentlyAssignedCourierIdsAsync();

        // 3. Faol va bo‘sh kuryerlarni filtrlash
        var availableCouriers = activeCouriers
            .Where(c => !busyCourierIds.Contains(c.UserId))
            .ToList();

        var courierDtos = availableCouriers.Select(c => MapUserService.MapToUserDto(c)).ToList();
        return courierDtos;

    }

    public async Task<List<UserDto>> GetAllUsersAsync(int skip, int take)
    {
        if (skip < 0 || take <= 0)
        {
            throw new ArgumentOutOfRangeException("Invalid pagination parameters.");
        }
        var users = await _userRepository.SelectAllAsync(skip, take);
        if (users == null || !users.Any())
        {
            return new List<UserDto>();
        }
        var userDtos = users.Select(u => MapUserService.MapToUserDto(u)).ToList();
        return userDtos;

    }

    public async Task<UserDto> GetUserByIdAsync(long userId)
    {
        var user = await _userRepository.SelectByIdAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with ID {userId} not found.");
        }
        var userDto = MapUserService.MapToUserDto(user);
        return userDto;
    }

    public Task UpdateUserAsync(UserDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeactivateUserAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllRolesAsync();
        if (roles == null || !roles.Any())
        {
            return new List<RoleDto>();
        }
        var roleDtos = roles.Select(r => new RoleDto
        {
            RoleId = r.RoleId,
            RoleName = r.Name,
            Description=r.Description
        }).ToList();                                                               
        return roleDtos;
    }

    public async Task<AdminDashboardStatsDto> GetAdminDashboardStatsAsync()
    {
        // Barcha foydalanuvchilar soni
        var totalUsers = await _userRepository.GetCountUsersAsync();

        // Barcha buyurtmalar soni
        var totalOrders = await _orderRepository.GetCountOrdersAsync();

        // Faol kuryerlar soni
        var activeCouriers = await _userRepository.CountActiveCouriersAsync();

        // Tayinlanmagan buyurtmalar soni
        var unassignedOrders = await _orderRepository.GetCountUnassingnAsync();

        return new AdminDashboardStatsDto
        {
            TotalClients = totalUsers,
            TotalOrders = totalOrders,
            ActiveCouriers = activeCouriers,
            UnassignedOrders = unassignedOrders
        };
    }

    public async Task<List<OrderDto>> GetOrdersByCourierAsync(long courierId)
    {
        var orders = await _orderRepository.GetOrdersByCourierAsync(courierId);
        if (orders?.Any() != true)
            return new List<OrderDto>();

        var ordersDto = orders.Select(o => MapOrderService.MapToOrderDto(o)).ToList();
        return ordersDto;
    }
    public async Task<List<UserDto>> GetUsersByRoleAsync(string role)
    {
        var users = await _userRepository.SelectUsersByRoleAsync(role);
        if (users == null || !users.Any())
        {
            return new List<UserDto>();
        }
        var userDtos = users.Select(u => MapUserService.MapToUserDto(u)).ToList();
        return userDtos;
    }

}
