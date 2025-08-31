using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class UserCreateDto
{
    public string FirstName { get; set; }         // Ism
    public string? LastName { get; set; }          // Familiya
    public string UserName { get; set; }          // Login uchun username
    public string? Email { get; set; }             // Email
    public string PhoneNumber { get; set; }       // Telefon raqam
    public string Password { get; set; }          // Parol (plain text, backendda hash qilinadi)
   /* public long RoleId { get; set; }         */      // Foydalanuvchi roli (Admin, Courier, Client va h.k.)

    // Agar mijoz uchun manzil kerak bo‘lsa:
    public string Address { get; set; }            // Manzil (ixtiyoriy)
    public double? Latitude { get; set; }          // Geokoordinatalar (ixtiyoriy)
    public double? Longitude { get; set; }         // Geokoordinatalar (ixtiyoriy)
}

