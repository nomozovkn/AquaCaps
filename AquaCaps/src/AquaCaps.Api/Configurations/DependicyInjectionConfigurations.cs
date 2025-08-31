using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using AquaCaps.Application.Interface;
using AquaCaps.Application.Services.AdminService;
using AquaCaps.Application.Services.AuthService;
using AquaCaps.Application.Services.OrderService;
using AquaCaps.Application.Services.RoleService;
using AquaCaps.Application.Services.TokenService;
using AquaCaps.Application.Services.UserService;
using AquaCaps.Application.Validators;
using AquaCaps.Infrastructure.Persistance.Repositories;
using FluentValidation;

namespace AquaCaps.Api.Configurations;

public static class DependicyInjectionConfigurations
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IOrderService, OrderService>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<IValidator<UserCreateDto>, UserCreateValidators>();
        builder.Services.AddScoped<IValidator<UserLogInDto>, UserLoginValidators>();

        builder.Services.AddScoped<IValidator<OrderCreateDto>, OrderCreateValidators>();
        builder.Services.AddScoped<IValidator<OrderUpdateDto>, OrderUpdateValidators>();
        builder.Services.AddScoped<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IOrderAssignmentRepository, OrderAssignmentRepository>();
        builder.Services.AddScoped<IOrderAssignmentService, OrderAssignmentService>();
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<ICourierRepository, CourierRepository>();
        builder.Services.AddScoped<ICourierService, CourierService>();

    }
}
