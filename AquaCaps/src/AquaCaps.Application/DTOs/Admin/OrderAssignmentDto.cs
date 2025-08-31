using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs.Admin;

public class OrderAssignmentDto          //zakazni kuryerga biriktirish uchun DTO
{
    public long Id { get; set; }               // Biriktirish ID si
    public long OrderId { get; set; }          // Buyurtma ID si
    public long CourierId { get; set; }        // Kuryer ID si
    public string CourierName { get; set; }    // Kuryer ismi (yoki UserName)
    public DateTime AssignedAt { get; set; }   // Biriktirilgan sana va vaqt
    public bool IsActive { get; set; }         // Hozirgi aktiv biriktirishmi (ha/yo'q)
}
