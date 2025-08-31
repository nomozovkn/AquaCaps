using AquaCaps.Application.DTOs;
using AquaCaps.Application.Services.RoleService;

namespace AquaCaps.Api.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        var roleGroup = app.MapGroup("/roles")
            .WithTags("RoleEndpoints");

        // POST - Add Role
        roleGroup.MapPost("/", async (RoleDto roleDto, IRoleService roleService) =>
        {
            var roleId = await roleService.AddRoleAsync(roleDto);
            return Results.Created($"/roles/{roleId}", roleId);
        })
        .WithName("AddRole")
        .Produces<long>(201);

        // GET - Get All Roles
        roleGroup.MapGet("/", async (IRoleService roleService) =>
        {
            var roles = await roleService.GetAllRolesAsync();
            return Results.Ok(roles);
        })
        .WithName("GetAllRoles")
        .Produces<ICollection<RoleDto>>(200);

        // PUT - Update Role
        roleGroup.MapPut("/{roleId:long}", async (long roleId, string newRoleName, IRoleService roleService) =>
        {
            await roleService.UpdateRoleAsync(roleId, newRoleName);
            return Results.NoContent();
        })
        .WithName("UpdateRole")
        .Produces(204);

        // DELETE - Delete Role
        roleGroup.MapDelete("/{roleId:long}", async (long roleId, IRoleService roleService) =>
        {
            await roleService.DeleteRoleAsync(roleId);
            return Results.NoContent();
        })
        .WithName("DeleteRole")
        .Produces(204);
    }

}
