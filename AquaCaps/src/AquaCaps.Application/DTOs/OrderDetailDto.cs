using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class OrderDetailDto
{
    public long OrderId { get; set; }
    public int OrderedCapsuleCount { get; set; }
    public string? Note { get; set; }

    // Client haqida ma'lumotlar
    public string ClientFirstName { get; set; }
    public string ClientLastName { get; set; }
    public string ClientPhoneNumber { get; set; }

    // Yetkazib berish manzili va koordinatalar
    public string DeliveryAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public DateTime CreatedAt { get; set; }
    // Boshqa kerakli maydonlar
}
