using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class OrderCreateDto
{
    
    public int OrderedCapsuleCount { get; set; }  // Buyurtma qilingan kapsula soni
    public DateOnly DeliveryDate { get; set; }     // Yetkazib berish sanasi
    public string? Address { get; set; }            // Yetkazib berish manzili
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Buyurtma yaratilgan sana va vaqt
    public int? ReturnedCapsuleCount { get; set; }  // Qaytarilgan bo‘sh kapsula soni (ixtiyoriy)
    public string? Note { get; set; }              // Qo‘shimcha izoh (ixtiyoriy)
}

