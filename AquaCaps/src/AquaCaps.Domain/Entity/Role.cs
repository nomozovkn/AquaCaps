using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Domain.Entity;

public class Role
{
    public long RoleId { get; set; }
    public string Name { get; set; } 
    public string Description { get; set; }
    public ICollection<User> Users { get; set; } // Navigation property to User entity, representing the users with this role

}
