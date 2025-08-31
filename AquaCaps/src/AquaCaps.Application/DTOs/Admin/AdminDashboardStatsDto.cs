using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs.Admin;

public class AdminDashboardStatsDto
{
    public int TotalOrders { get; set; }            // Umumiy buyurtmalar soni
    public int PendingOrders { get; set; }          // Hozirda bajarilayotgan buyurtmalar soni
    public int CompletedOrders { get; set; }        // Bajarilgan buyurtmalar soni
    public int CancelledOrders { get; set; }        // Bekor qilingan buyurtmalar soni

    public int TotalClients { get; set; }           // Umumiy mijozlar soni
    public int ActiveClients { get; set; }          // Faol mijozlar soni
    public int InactiveClients { get; set; }        // Faol bo‘lmagan mijozlar soni

    public int TotalCouriers { get; set; }          // Umumiy kuryerlar soni
    public int ActiveCouriers { get; set; }         // Faol kuryerlar soni
    public int InactiveCouriers { get; set; }       // Faol bo‘lmagan kuryerlar soni
    public int UnassignedOrders { get; set; }       // Tayinlanmagan buyurtmalar soni
}
