using AquaCaps.Application.DTOs;
using AquaCaps.Application.Interface;
using AquaCaps.Application.MapService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderAssignmentRepository _orderAssignmentRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public UserService(IUserRepository userRepository,IRoleRepository roleRepository,IOrderRepository orderrepository,IOrderAssignmentRepository orderassignmnetRepository,IRefreshTokenRepository refreshTokenRepasitory)
    {
        _orderAssignmentRepository = orderassignmnetRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _orderRepository = orderrepository;
        _refreshTokenRepository = refreshTokenRepasitory;

    }
  
}
