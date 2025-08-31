using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs;

public class OrderUpdateDto
{
    public long OrderId { get; set; }
    public string Address { get; set; }
    public int OrderedCapsuleCount { get; set; }
    public int? ReturnedCapsuleCount { get; set; } 
    public DateOnly DeliveryDate { get; set; }
    public string? Note { get; set; }
}