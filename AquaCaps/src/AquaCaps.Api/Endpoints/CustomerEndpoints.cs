using AquaCaps.Application.DTOs;
using AquaCaps.Application.Services.UserService;
using Microsoft.AspNetCore.Authorization;

namespace AquaCaps.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var customerGroup = app.MapGroup("/customer")
            //.RequireAuthorization("CustomerPolicy")
            .WithTags("CustomerEndpoints");
        customerGroup.MapPost("/create-order", [Authorize (Roles = "User")]
        async (OrderCreateDto orderDto, ICustomerService customerService, HttpContext httpContext) =>
            {
                var clientIdClaim = httpContext.User.FindFirst("UserId")?.Value;
                if (clientIdClaim == null)
                    return Results.Unauthorized();

                long clientId = long.Parse(clientIdClaim);

                var orderId = await customerService.CreateOrderAsync(orderDto, clientId);
                return Results.Ok(orderId);
            })
            .WithName("CreateOrder")
            .Produces<long>(200)
            .Produces(400);
        customerGroup.MapGet("/active-orders",
            async (HttpContext httpContext, ICustomerService customerService) =>
            {
                var clientIdClaim = httpContext.User.FindFirst("UserId")?.Value;
                if (clientIdClaim == null)
                    return Results.Unauthorized();
                long clientId = long.Parse(clientIdClaim);
                var orders = await customerService.GetAllActiveOrdersAsync(clientId);
                return Results.Ok(orders);
            })
            .WithName("GetAllActiveOrders")
            .Produces<ICollection<OrderDto>>(200)
            .Produces(404);
        customerGroup.MapGet("/order-history",
            async (int skip, int take, HttpContext httpContext, ICustomerService customerService) =>
            {
                var clientIdClaim = httpContext.User.FindFirst("UserId")?.Value;
                if (clientIdClaim == null)
                    return Results.Unauthorized();
                long clientId = long.Parse(clientIdClaim);
                var orders = await customerService.GetOrdersHistoryAsync(skip, take, clientId);
                return Results.Ok(orders);
            })
            .WithName("GetOrdersHistory")
            .Produces<List<OrderDto>>(200);
        //.Produces(404);
        customerGroup.MapPut("/update", [Authorize(Roles = "User")]
        async (OrderUpdateDto orderUpdateDto, HttpContext httpContext, ICustomerService customerService) =>
            {
                var clientIdClaim = httpContext.User.FindFirst("UserId")?.Value;
                if (clientIdClaim == null)
                    return Results.Unauthorized();
                long clientId = long.Parse(clientIdClaim);

                await customerService.UpdateAsync(orderUpdateDto);
                return Results.Ok();
            })
            .WithName("UpdateOrder")
            .Produces(200)
            .Produces(400);
        customerGroup.MapDelete("/delete-order/{orderId}",
            async (long orderId, HttpContext httpContext, ICustomerService customerService) =>
            {
                var clientIdClaim = httpContext.User.FindFirst("UserId")?.Value;
                if (clientIdClaim == null)
                    return Results.Unauthorized();
                long clientId = long.Parse(clientIdClaim);
                await customerService.DeleteAsync(orderId);
                return Results.Ok();
            })
            .WithName("DeleteOrder")
            .Produces(200)
            .Produces(404);

    }
}
