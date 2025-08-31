using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Domain.Entity;

public class OrderAssignment
{
    public long Id { get; set; }  // Primary Key

    public long OrderId { get; set; }  // Foreign Key to Order
    public Order Order { get; set; }   // Navigation property

    public long CourierId { get; set; }  // Foreign Key to User (Courier)
    public User Courier { get; set; }     // Navigation property

    public DateTime AssignedAt { get; set; }  // Qachon biriktirilgan

    public bool IsActive { get; set; }  // Hozirgi aktiv biriktirishmi
}
