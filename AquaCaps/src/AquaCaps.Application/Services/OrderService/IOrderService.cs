using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using System.Linq;

namespace AquaCaps.Application.Services.OrderService;


public interface IOrderService
{
    // === CRUD for Admin ===

    Task<OrderDto> GetByIdAsync(long orderId); // Buyurtma ID orqali buyurtma ma’lumotini olish // Admin, Courier, Client
    Task<IEnumerable<OrderDto>> GetAllAsync(int skip,int take); // Barcha buyurtmalarni olish (faqat admin) // Admin
    //Task<long> CreateAsync(CreateOrderDto orderDto); // Yangi buyurtma yaratish // Admin
    Task<long> CreateOrderAsync(OrderCreateDto orderDto); // Yangi buyurtma yaratish Client
    Task UpdateAsync(OrderUpdateDto orderDto); // Mavjud buyurtmani tahrirlash // Admin
    Task DeleteAsync(long orderId); // Buyurtmani o‘chirish // Admin
    Task CancelClientActiveOrderAsync(string clientPhone); // Admin telefon raqami orqali faol buyurtmani bekor qiladi


    // === Assignment ===

    //Task AssignToCourierAsync(long orderId, long courierId); // Buyurtmani kuryerga qo‘lda biriktirish // Admin
    Task AutoAssignPendingOrdersAsync(); // Tizim avtomatik buyurtmalarni kuryerlarga taqsimlaydi // Admin
    //Task LockAssignmentAsync(long orderId); // Buyurtmani assignment uchun bloklash // Admin yoki tizim
    //Task UnlockAssignmentAsync(long orderId); // Buyurtma blokini yechish // Admin
    //Task<bool> IsOrderLockedAsync(long orderId); // Buyurtma bloklanganligini tekshirish // Admin, tizim

    // === Kuryer uchun ===

    //Task<IEnumerable<OrderDto>> GetCourierOrdersAsync(long courierId); // Kuryerga tegishli buyurtmalar ro‘yxati // Courier
    Task ConfirmDeliveryAsync(long orderId, int returnedCapsules); // Buyurtmani yetkazib berilgan deb tasdiqlash + qaytgan kapsulalar // Courier
    Task MarkAsReturnedAsync(long orderId); // Buyurtma qaytarilgan deb belgilash (mijoz qabul qilmagan) // Courier

    // === Mijozlar bilan ishlash ===

    IQueryable<OrderDto> GetClientOrdersAsync(long clientId); // Mijozga tegishli buyurtmalarni olish // Client
    Task<IEnumerable<UserDto>> GetInactiveClientsOrdersAsync(DateTime sinceDate); //shu vaqtdan beri faol bolmagan mijozlar


    // === Statistikalar ===

    Task<OrderStatsDto> GetAdminOrderStatsAsync(); // Buyurtmalar statistikasi (jami, aktiv, tugagan va h.k.) // Admin
    //Task<OrderStatsDto> GetCourierOrderStatsAsync(long courierId); // Kuryerga oid statistikalar // Courier

    // === Filtrlash, qidiruv ===

    //Task<IEnumerable<OrderDto>> FilterOrdersAsync(OrderFilterDto filter); // Filter orqali buyurtmalarni olish (status, sana va h.k.) // Admin
    //Task<IEnumerable<OrderDto>> SearchOrdersAsync(string keyword); // Kalit so‘z orqali buyurtmalarni qidirish (ism, telefon, manzil) // Admin
}
