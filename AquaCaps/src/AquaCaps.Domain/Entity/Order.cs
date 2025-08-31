using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Domain.Entity;

public class Order
{
    public long OrderId { get; set; }
    public bool IsDelevered { get; set; } // Yetkazib berilganligini bildiruvchi flag
    public bool IsCancelled { get; set; } // Buyurtma bekor qilinganligini bildiruvchi flag
    public long CreatedByUserId { get; set; } // FK
    public User CreatedByUser { get; set; }
    public long? AssignedCourierId { get; set; } // FK
    public User AssignedCourier { get; set; }
    public int OrderedCapsuleCount { get; set; }      // Nechta to‘la kapsula yetkazib berildi
    public int? ReturnedCapsuleCount { get; set; }    // Nechta bo‘sh kapsula qaytarildi
    public DateTime CreatedAt { get; set; }
    public DateTime? AssignedAt { get; set; } // Nullable to allow for unassigned orders
    public ICollection<OrderAssignment> OrderAssignments { get; set; }
    public string ? Note { get; set; } // Qo‘shimcha izoh (ixtiyoriy)
    public DateOnly DeliveryDate { get; set; } // Yetkazib berish sanasi
    public string Address { get; set; } // Yetkazib berish manzili
}
