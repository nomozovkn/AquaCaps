using AquaCaps.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;

namespace AquaCaps.Api.Configurations;

public static class DataBaseConfigurations
{
    public static void ConfigureDB(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(connectionString));
    }
}