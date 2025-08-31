using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Infrastructure.Persistance.Configurations;

public class OrderAssignmentConfiguration : IEntityTypeConfiguration<OrderAssignment>
{
    public void Configure(EntityTypeBuilder<OrderAssignment> builder)
    {
        builder.ToTable("OrderAssignments");

        builder.HasKey(oa => oa.Id);

        builder.Property(oa => oa.AssignedAt)
            .IsRequired();

        builder.Property(oa => oa.IsActive)
            .IsRequired();

        // Order bilan bog‘lanish
        builder.HasOne(oa => oa.Order)
            .WithMany(o => o.OrderAssignments)
            .HasForeignKey(oa => oa.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Courier bilan bog‘lanish
        builder.HasOne(oa => oa.Courier)
            .WithMany(u => u.OrderAssignments) // User.OrderAssignments bilan bog‘lanadi
            .HasForeignKey(oa => oa.CourierId)
            .OnDelete(DeleteBehavior.Restrict); // Kuryer o‘chsa, assignmentlar saqlanadi

    }
}
