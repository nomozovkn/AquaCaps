using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using AquaCaps.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.MapService;

public static class MapOrderService
{
    public static OrderAssignmentDto MapToDto(OrderAssignment assignment)
    {
        return new OrderAssignmentDto
        {
            Id = assignment.Id,
            OrderId = assignment.OrderId,
            CourierId = assignment.CourierId,
            CourierName = assignment.Courier.UserName ?? "", // yoki FullName
            AssignedAt = assignment.AssignedAt,
            IsActive = true // Agar kerak bo‘lsa, qo‘shimcha shart qo‘yish mumkin
        };
    }
    public static Order MapToOrderEntity(CreateOrderDto dto, long createdByUserId)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        return new Order
        {
            CreatedByUserId = createdByUserId,
            OrderedCapsuleCount = dto.CapsuleCount,
            CreatedAt = DateTime.UtcNow,
            DeliveryDate = dto.DeliveryDate,
            Address = dto.Address,
            // AssignedCourierId, AssignedAt, ReturnedCapsuleCount, Note
            // boshlang'ich qiymatlar uchun null yoki default qoldiriladi
        };
    }


    public static OrderDto MapToOrderDto(Order order)
    {
        if (order == null) return null!;

        return new OrderDto
        {
            OrderId = order.OrderId,
            CreatedByUserId = order.CreatedByUserId,
            CreatedByUserName = order.CreatedByUser?.UserName ?? "",
            AssignedCourierId = order.AssignedCourierId,
            AssignedCourierName = order.AssignedCourier?.UserName ?? "",
            ClientId = order.CreatedByUserId,
            ClientName = order.CreatedByUser?.FirstName + " " + order.CreatedByUser?.LastName,
            ClientAddress = order.Address ?? "",
            ClientPhoneNumber = order.CreatedByUser?.PhoneNumber ?? "",
            OrderedCapsuleCount = order.OrderedCapsuleCount,
            ReturnedCapsuleCount = order.ReturnedCapsuleCount,
            CreatedAt = order.CreatedAt,
            AssignedAt = order.AssignedAt,
            IsDelevered = order.IsDelevered,
            IsCancelled = order.IsCancelled

        };
    }

    public static Order MapToOrderEntity(OrderDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        return new Order
        {
            AssignedCourierId = dto.AssignedCourierId,
            OrderedCapsuleCount = dto.OrderedCapsuleCount,
            ReturnedCapsuleCount = dto.ReturnedCapsuleCount,
            AssignedAt = dto.AssignedAt,
            Address = dto.ClientAddress,
            CreatedByUserId = dto.CreatedByUserId,
            CreatedAt = dto.CreatedAt,
            IsDelevered = dto.IsDelevered,
            IsCancelled = dto.IsCancelled,
            // Agar kerak bo‘lsa, boshqa maydonlarni ham qo‘shing
        };
    }
    public static Order MapToOrder(OrderUpdateDto dto)
    {
        return new Order
        {
            OrderId = dto.OrderId,
            Address = dto.Address,
            OrderedCapsuleCount = dto.OrderedCapsuleCount,
            ReturnedCapsuleCount = dto.ReturnedCapsuleCount,
            DeliveryDate = dto.DeliveryDate,
            Note = dto.Note,

            // Quyidagilar yo‘q, chunki DTO’da yo‘q:
            // OrderId
            // CreatedByUserId
            // CreatedAt
            // AssignedCourierId
            // AssignedAt
            // OrderAssignments
        };
    }
    public static Order MapToOrder(OrderCreateDto dto)
    {
        return new Order
        {
            OrderedCapsuleCount = dto.OrderedCapsuleCount,
            Note = dto.Note,
            DeliveryDate = dto.DeliveryDate,
            ReturnedCapsuleCount = dto.ReturnedCapsuleCount ?? 0, // agar null bo‘lsa 0 qo‘yamiz
            IsDelevered = false,
            IsCancelled = false,
            CreatedAt = DateTime.UtcNow,
            OrderAssignments = new List<OrderAssignment>()
        };


    }
}
