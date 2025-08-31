using AquaCaps.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Interface;

public interface IOrderAssignmentRepository
{
    public Task AssignOrderAsync(long orderId,long courierId); // Buyurtmani berilgan kuryerga biriktiradi
    Task UnassignOrderAsync(Order order); // Buyurtmani bo'shatadi (tayinlangan kuryerni olib tashlaydi)
    Task<OrderAssignment?> GetAssignmentByOrderIdAsync(long orderId); // Buyurtmaga tayinlangan kuryerni qaytaradi
    Task<IEnumerable<OrderAssignment>> GetAssignmentsByCourierIdAsync(long courierId); // Kuryerga biriktirilgan barcha buyurtmalar ro'yxatini qaytaradi
    Task<bool> IsOrderAssignedAsync(long orderId); // Buyurtma tayinlangan yoki yo'qligini tekshiradi
    public Task<List<long>> GetCurrentlyAssignedCourierIdsAsync();// Hozirda buyurtmaga tayinlangan kuryerlar ID larini qaytaradi
    Task<int> GetCountOrderAssignmentAsync();
}
