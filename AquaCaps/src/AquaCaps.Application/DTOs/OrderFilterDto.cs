using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class OrderFilterDto
{
    public string? Status { get; set; } // Buyurtma holati (masalan, "Active", "Completed", "Cancelled")
    public DateTime? StartDate { get; set; } // Boshlanish sanasi
    public DateTime? EndDate { get; set; } // Tugash sanasi
    public string? ClientPhone { get; set; } // Mijozning telefon raqami
    public string? CourierId { get; set; } // Kuryer IDsi
    public string? SearchKeyword { get; set; } // Qidiruv kalit so‘zi (ism, manzil va h.k.)
}
