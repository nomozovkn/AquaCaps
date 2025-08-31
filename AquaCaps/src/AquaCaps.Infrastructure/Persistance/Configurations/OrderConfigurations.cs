using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Infrastructure.Persistance.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.OrderId);

        builder.Property(o => o.OrderedCapsuleCount)
            .IsRequired();

        builder.Property(o => o.ReturnedCapsuleCount)
            .IsRequired(false);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.AssignedAt)
            .IsRequired(false);

        // CreatedByUser FK
        builder.HasOne(o => o.CreatedByUser)
            .WithMany(u => u.Orders)            // User.Orders ga bog‘lanadi
            .HasForeignKey(o => o.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Foydalanuvchi o‘chsa, buyurtma o‘chmasin

        // AssignedCourier FK (nullable)
        builder.HasOne(o => o.AssignedCourier)
            .WithMany(u => u.AssignedOrders)    // User.AssignedOrders ga bog‘lanadi
            .HasForeignKey(o => o.AssignedCourierId)
            .OnDelete(DeleteBehavior.SetNull); // Kuryer o‘chsa, buyurtma saqlanadi

        // OrderAssignments bog‘lanishi (1 Order ↔ ko‘p OrderAssignment)
        builder.HasMany(o => o.OrderAssignments)
            .WithOne(oa => oa.Order)
            .HasForeignKey(oa => oa.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // Buyurtma o‘chsa, assignmentlar ham o‘chadi
    }

}
