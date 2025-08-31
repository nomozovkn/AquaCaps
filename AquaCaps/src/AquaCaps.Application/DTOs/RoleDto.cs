namespace AquaCaps.Application.DTOs;

public class RoleDto
{
    public long RoleId { get; set; } // Rolning unikal identifikatori
    public string RoleName { get; set; } // Rol nomi (masalan, "Admin", "Courier", "Customer")
    public string Description { get; set; } // Rol haqida qisqacha tavsif
}
