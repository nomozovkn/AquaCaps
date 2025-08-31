using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class OrderDto
{
    public long OrderId { get; set; }
    public bool IsDelevered { get; set; } // Yetkazib berilganligini bildiruvchi flag
    public bool IsCancelled { get; set; } // Buyurtma bekor qilinganligini bildiruvchi flag

    // Buyurtmani yaratgan foydalanuvchi haqida
    public long CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; }  // Masalan, "Ism Familiya" yoki username

    // Buyurtmaga biriktirilgan kuryer haqida
    public long? AssignedCourierId { get; set; }
    public string AssignedCourierName { get; set; }  // Masalan, "Ism Familiya"

    // Mijoz (client) haqida
    public long ClientId { get; set; }
    public string ClientName { get; set; }
    public string ClientAddress { get; set; }
    public string ClientPhoneNumber { get; set; }

    public int OrderedCapsuleCount { get; set; }
    public int? ReturnedCapsuleCount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public string? Note { get; set; } // Qo'shimcha izoh yoki eslatma
    public DateTime? DeliveredAt { get; set; } // Agar buyurtma yetkazilgan bo'lsa, bu sana saqlanadi
    public DateTime? CancelledAt { get; set; } // Agar buyurtma bekor qilingan bo'lsa, bu sana saqlanadi
    public string Address { get; set; } // Yetkazib berish manzili
    public double? Latitude { get; set; } // Yetkazib berish manzilining kenglik koordinatasi
    public double? Longitude { get; set; } // Yetkazib berish manzilining uzunlik koordinatasi

}
