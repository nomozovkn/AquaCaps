using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs.Admin;

public class CreateOrderDto
{
    
    public string FirstName { get; set; }   // Mijoz ismi

    
    public string? LastName { get; set; }    // Mijoz familiyasi

   
    public string PhoneNumber { get; set; } // Telefon raqam

    
    //public string Role { get; set; } = "User"; // Mijoz roli (default "User")
    public string Address { get; set; }     // Yetkazib berish manzili

    
    public int CapsuleCount { get; set; }   // Yetkazib beriladigan kapsulalar soni

    
    public DateOnly DeliveryDate { get; set; } // Yetkazib berish sanasi
}
