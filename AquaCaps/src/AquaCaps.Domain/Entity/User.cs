namespace AquaCaps.Domain.Entity;

public class User
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string UserName { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Salt { get; set; }
    public bool IsActive { get; set; }
    public string Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public long RoleId { get; set; }
    public Role Role { get; set; }

    // Mijoz sifatida berilgan buyurtmalar
    public ICollection<Order> Orders { get; set; }

    // Kuryer sifatida biriktirilgan buyurtmalar (agar AssignedCourier sifatida saqlansa)
    public ICollection<Order> AssignedOrders { get; set; }

    // OrderAssignment orqali biriktirilgan buyurtmalar (tarixiy biriktirishlar)
    public ICollection<OrderAssignment> OrderAssignments { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
