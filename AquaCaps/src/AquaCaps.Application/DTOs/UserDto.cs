using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class UserDto
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string Address { get; set; }    // Mijozning manzili (shahar, ko‘cha, uy raqami va h.k.)
    public double? Latitude { get; set; }  // Agar kerak bo‘lsa, geografik kenglik
    public double? Longitude { get; set; } // Agar kerak bo‘lsa, geografik uzunlik
    public string RoleName => Role?.RoleName;
    public RoleDto Role { get; set; }
    //public string RoleName { get; set; }  // Rol nomi (Admin, Courier va hokazo)
}

