using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Domain.Entity;

public class AdminDashboardStats
{
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }

    public int TotalClients { get; set; }
    public int ActiveClients { get; set; }
    public int InactiveClients { get; set; }

    public int TotalCouriers { get; set; }
    public int ActiveCouriers { get; set; }
    public int InactiveCouriers { get; set; }
}
