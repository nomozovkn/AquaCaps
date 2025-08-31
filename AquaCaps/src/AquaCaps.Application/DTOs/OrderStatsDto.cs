using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class OrderStatsDto
{
    // Umumiy statistika
    public int TotalOrders { get; set; }                // Jami buyurtmalar soni
    public int AssignedOrders { get; set; }             // Kuryerga biriktirilgan buyurtmalar
    public int UnassignedOrders { get; set; }           // Hali kuryerga biriktirilmagan buyurtmalar
    public int CompletedOrders { get; set; }            // Yakunlangan (yetkazilgan) buyurtmalar
    public int CancelledOrders { get; set; }            // Bekor qilingan buyurtmalar

    // Bugungi kun statistikasi
    public int OrdersToday { get; set; }                // Bugun yaratilgan buyurtmalar
    public int CompletedToday { get; set; }             // Bugun yakunlangan buyurtmalar
    public int NewClientsToday { get; set; }            // Bugun birinchi marta buyurtma qilgan mijozlar

    // So'nggi 7 kun ichida
    public int OrdersLast7Days { get; set; }

    // Kuryerlar faoliyati
    public int ActiveCouriers { get; set; }             // Faol kuryerlar soni
    public int InactiveCouriers { get; set; }           // So‘nggi kunlarda hech qanday buyurtma olmaganlar

    // Mijozlar bilan bog‘liq
    public int ActiveClients { get; set; }              // So‘nggi 30 kunda kamida 1 marta buyurtma qilganlar
    public int InactiveClients { get; set; }            // Faol bo‘lmagan mijozlar

    // Optional: vaqt bo‘yicha trend
    
}
