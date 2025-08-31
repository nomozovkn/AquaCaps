using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.DTOs.Admin;

public class AssignOrderDto
{
    [Required]
    public long OrderId { get; set; }

    [Required]
    public long CourierId { get; set; }
}
