using AquaCaps.Application.DTOs.Admin;

namespace AquaCaps.Application.Services.AdminService;

public interface IOrderAssignmentService
{
    Task AssignOrderToCourierAsync(long orderId, long courierId); // Buyurtmani berilgan kuryerga biriktiradi
    Task UnassignOrderAsync(long orderId); // Buyurtmani bo'shatadi (tayinlangan kuryerni olib tashlaydi)
    Task<OrderAssignmentDto?> GetAssignmentByOrderIdAsync(long orderId); // Buyurtmaga tayinlangan kuryer haqida ma'lumotni qaytaradi
    Task<IEnumerable<OrderAssignmentDto>> GetAssignmentsByCourierIdAsync(long courierId); // Kuryerga biriktirilgan barcha buyurtmalar ro'yxatini qaytaradi
    Task<bool> IsOrderAssignedAsync(long orderId); // Buyurtma tayinlangan yoki yo'qligini tekshiradi
}
