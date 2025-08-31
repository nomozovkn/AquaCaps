using AquaCaps.Application.DTOs;
using AquaCaps.Application.Services.AdminService;
using AquaCaps.Application.Services.OrderService;

namespace AquaCaps.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/admin")
            .WithTags("AdminEndpoints");


        adminGroup.MapPost("/add-admin-or-courier/{userId}/{roleId}",
            async (long userId, long roleId, IAdminService adminService) =>
            {
                await adminService.AddAdminOrCourier(userId, roleId);
                return Results.Ok();
            })
            .WithName("AddAdminOrCourier")
            .Produces(200)
            .Produces(400);

        adminGroup.MapPost("/assign-order/{orderId}/{courierId}",
            async (long orderId, long courierId, IAdminService adminService) =>
            {
                var result = await adminService.AssignOrderToCourierAsync(orderId, courierId);
                return result ? Results.Ok() : Results.BadRequest("Order is already assigned to a courier.");
            })
            .WithName("AssignOrderToCourier")
            .Produces(200)
            .Produces(400);

        adminGroup.MapPost("/unassign-courier/{orderId}",
            async (long orderId, IAdminService adminService) =>
            {
                var result = await adminService.UnassignCourierAsync(orderId);
                return result ? Results.Ok() : Results.BadRequest("Order is not assigned to any courier.");
            })
            .WithName("UnassignCourier")
            .Produces(200)
            .Produces(400);

        //adminGroup.MapPost("/create-order",
        //    async (CreateOrderDto orderDto, IAdminService adminService) =>
        //    {
        //        var orderId = await adminService.CreateOrderAsync(orderDto);
        //        return Results.Ok(orderId);
        //    })
        //    .WithName("CreateOrder")
        //    .Produces<long>(200)
        //    .Produces(400);

        adminGroup.MapPost("/cancel-active-order/{clientPhone}",
            async (string clientPhone, IOrderService orderService) =>
            {
                await orderService.CancelClientActiveOrderAsync(clientPhone);
                return Results.Ok();
            })
            .WithName("CancelClientActiveOrder")
            .Produces(200)
            .Produces(404);

        adminGroup.MapPost("/auto-assign-pending-orders",
            async (IOrderService orderService) =>
            {
                await orderService.AutoAssignPendingOrdersAsync();
                return Results.Ok();
            })
            .WithName("AutoAssignPendingOrders")
            .Produces(200)
            .Produces(400);

        adminGroup.MapPost("/confirm-delivery/{orderId}/{returnedCapsules}",
            async (long orderId, int returnedCapsules, IOrderService orderService) =>
            {
                await orderService.ConfirmDeliveryAsync(orderId, returnedCapsules);
                return Results.Ok();
            })
            .WithName("ConfirmDelivery")
            .Produces(200)
            .Produces(404);

        adminGroup.MapPost("/mark-as-returned/{orderId}",
            async (long orderId, IOrderService orderService) =>
            {
                await orderService.MarkAsReturnedAsync(orderId);
                return Results.Ok();
            })
            .WithName("MarkAsReturned")
            .Produces(200)
            .Produces(404);


        adminGroup.MapGet("/users/{skip}/{take}",
            async (int skip, int take, IAdminService adminService) =>
            {
                var users = await adminService.GetAllUsersAsync(skip, take);
                return Results.Ok(users);
            })
            .WithName("GetAllUsers")
            .Produces(200)
            .Produces(400);

        adminGroup.MapGet("/user/{userId}",
            async (long userId, IAdminService adminService) =>
            {
                var user = await adminService.GetUserByIdAsync(userId);
                return Results.Ok(user);
            })
            .WithName("GetUserById")
            .Produces(200)
            .Produces(404);

        adminGroup.MapGet("/orders",
            async (IAdminService adminService) =>
            {
                var orders = await adminService.GetAllOrdersAsync();
                return Results.Ok(orders);
            })
            .WithName("GetAllOrders")
            .Produces(200)
            .Produces(400);
        adminGroup.MapGet("/usersByRole/{role}",
            async (string role, IAdminService adminService) =>
            {
                var users = await adminService.GetUsersByRoleAsync(role);
                return Results.Ok(users);
            })
            .WithName("GetUsersByRole")
            .Produces(200)
            .Produces(404);

        adminGroup.MapGet("/order/{orderId}",
            async (long orderId, IOrderService orderService) =>
            {
                var order = await orderService.GetByIdAsync(orderId);
                return Results.Ok(order);
            })
            .WithName("GetOrderById")
            .Produces(200)
            .Produces(404);

        adminGroup.MapGet("/couriers",
            async (IAdminService adminService) =>
            {
                var couriers = await adminService.GetAvailableCouriersAsync();
                return Results.Ok(couriers);
            })
            .WithName("GetAvailableCouriers")
            .Produces(200)
            .Produces(400);

        //adminGroup.MapGet("/roles",
        //    async (IAdminService adminService) =>
        //    {
        //        var roles = await adminService.GetAllRolesAsync();
        //        return Results.Ok(roles);
        //    })
        //    .WithName("GetAllRoles")
        //    .Produces(200)
        //    .Produces(400);

        adminGroup.MapGet("/dashboard-stats",
            async (IAdminService adminService) =>
            {
                var stats = await adminService.GetAdminDashboardStatsAsync();
                return Results.Ok(stats);
            })
            .WithName("GetAdminDashboardStats")
            .Produces(200)
            .Produces(400);

        adminGroup.MapGet("/orders-by-courier/{courierId}",
            async (long courierId, IAdminService adminService) =>
            {
                var orders = await adminService.GetOrdersByCourierAsync(courierId);
                return Results.Ok(orders);
            })
            .WithName("GetOrdersByCourier")
            .Produces(200)
            .Produces(404);

        adminGroup.MapGet("/inactive-clients/{sinceDate}",
            async (DateTime sinceDate, IOrderService orderService) =>
            {
                var clients = await orderService.GetInactiveClientsOrdersAsync(sinceDate);
                return Results.Ok(clients);
            })
            .WithName("GetInactiveClients")
            .Produces(200)
            .Produces(404);

        adminGroup.MapGet("/client-orders/{clientId}",
             (long clientId, IOrderService orderService) =>
             {
                 var orders = orderService.GetClientOrdersAsync(clientId);
                 return Results.Ok(orders);
             })
            .WithName("GetClientOrders")
            .Produces(200)
            .Produces(404);

        adminGroup.MapGet("/order-stats",
            async (IOrderService orderService) =>
            {
                var stats = await orderService.GetAdminOrderStatsAsync();
                return Results.Ok(stats);
            })
            .WithName("GetAdminOrderStats")
            .Produces(200)
            .Produces(400);


        adminGroup.MapPut("/update-user",
            async (UserDto userDto, IAdminService adminService) =>
            {
                await adminService.UpdateUserAsync(userDto);
                return Results.Ok();
            })
            .WithName("UpdateUser")
            .Produces(200)
            .Produces(400);

        adminGroup.MapPut("/update-order",
            async (OrderUpdateDto orderDto, IAdminService adminService) =>
            {
                await adminService.UpdateOrderAsync(orderDto);
                return Results.Ok();
            })
            .WithName("UpdateOrder")
            .Produces(200)
            .Produces(400);


        adminGroup.MapDelete("/deactivate-user/{userId}",
            async (long userId, IAdminService adminService) =>
            {
                var result = await adminService.DeactivateUserAsync(userId);
                return result ? Results.Ok() : Results.NotFound($"User with ID {userId} not found.");
            })
            .WithName("DeactivateUser")
            .Produces(200)
            .Produces(404);

        adminGroup.MapDelete("/delete-order/{orderId}",
            async (long orderId, IAdminService adminService) =>
            {
                await adminService.DeleteOrderAsync(orderId);
                return Results.Ok();
            })
            .WithName("DeleteOrder")
            .Produces(200)
            .Produces(404);
    }

}
