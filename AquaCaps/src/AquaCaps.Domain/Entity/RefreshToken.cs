using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Domain.Entity;

public class RefreshToken
{
    public long RefreshTokenId { get; set; }
    public string Token { get; set; }
    public DateTime Expire { get; set; }
    public bool IsRevoked { get; set; } // Indicates if the token is still valid
    public long UserId { get; set; }
    public User User { get; set; } // Navigation property to User entity

}
