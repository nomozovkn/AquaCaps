using AquaCaps.Application.DTOs;
using AquaCaps.Application.Interface;
using AquaCaps.Core.Errors;
using AquaCaps.Domain.Entity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Services.UserService;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<OrderCreateDto> _orderCreateValidator;
    private readonly IValidator<OrderUpdateDto> _orderUpdateValidator;
    public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository, IOrderRepository orderRepository, IValidator<OrderCreateDto> orderCreateValidator, IValidator<OrderUpdateDto> orderUpdateValidator)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _orderCreateValidator = orderCreateValidator;
        _orderUpdateValidator = orderUpdateValidator;
    }
    public async Task<long> CreateOrderAsync(OrderCreateDto dto, long clientId)
    {
        var order = new Order
        {
            CreatedByUserId = clientId,
            OrderedCapsuleCount = dto.OrderedCapsuleCount,
            DeliveryDate = dto.DeliveryDate,
            Address = dto.Address,
            Note = dto.Note,
            CreatedAt = DateTime.UtcNow
        };

        await _orderRepository.InsertAsync(order);

        return order.OrderId;
    }

    public  async Task DeleteAsync(long orderId)
    {
       var order = await _orderRepository.SelectByIdAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order with ID {orderId} not found.");
        }
         await _orderRepository.DeleteAsync(order);
    }

    public async Task<ICollection<OrderDto>> GetAllActiveOrdersAsync(long customerId)
    {
        var orders = await _orderRepository.SelectAll()
            .Where(o => o.CreatedByUserId == customerId && o.IsDelevered == false).ToListAsync();
        if (orders == null || !orders.Any())
        {
            return new List<OrderDto>();
        }

        return orders.Select(o => new OrderDto
        {
            OrderId = o.OrderId,
            CreatedByUserId = o.CreatedByUserId,
            OrderedCapsuleCount = o.OrderedCapsuleCount,
            DeliveryDate = o.DeliveryDate,
            Address = o.Address,
            Note = o.Note,
            CreatedAt = o.CreatedAt,
            IsDelevered = o.IsDelevered
        }).ToList();
    }

    public async Task<List<OrderDto>> GetOrdersHistoryAsync(int skip, int take, long customerId)
    {
        var orders=await _orderRepository.SelectAll()
            .Where(o => o.CreatedByUserId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        if (orders == null || !orders.Any())
            {
            return new List<OrderDto>();
        }
        return orders.Select(o => new OrderDto
        {
            OrderId = o.OrderId,
            CreatedByUserId = o.CreatedByUserId,
            OrderedCapsuleCount = o.OrderedCapsuleCount,
            DeliveryDate = o.DeliveryDate,
            Address = o.Address,
            Note = o.Note,
            CreatedAt = o.CreatedAt,
            IsDelevered = o.IsDelevered
        }).ToList();
    }

    public async Task UpdateAsync(OrderUpdateDto orderUpdateDto)
    {
        var isvalid = _orderUpdateValidator.Validate(orderUpdateDto);
        if (!isvalid.IsValid)
        {
            throw new ValidationException(isvalid.Errors);
        }

        
        var existingOrder = await _orderRepository.SelectByIdAsync(orderUpdateDto.OrderId);
        if (existingOrder == null)
        {
            throw new EntityNotFoundException($"Order with ID {orderUpdateDto.OrderId} not found.");
        }

       
        existingOrder.OrderedCapsuleCount = orderUpdateDto.OrderedCapsuleCount;
        existingOrder.Address = orderUpdateDto.Address;
        existingOrder.DeliveryDate = orderUpdateDto.DeliveryDate;
        existingOrder.Note = orderUpdateDto.Note;
        existingOrder.ReturnedCapsuleCount = orderUpdateDto.ReturnedCapsuleCount;

        
        await _orderRepository.UpdateAsync(existingOrder);
    }
}
