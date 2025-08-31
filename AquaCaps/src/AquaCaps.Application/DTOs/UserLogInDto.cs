using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class UserLogInDto
{
    public string UserName { get; set; }  // Foydalanuvchi nomi yoki email
    public string Password { get; set; }   // Parol (plain text, backendda hash qilinadi)
    //public bool RememberMe { get; set; }   // "Meni eslab qolish" opsiyasi (ixtiyoriy)
}
