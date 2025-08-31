using AquaCaps.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Infrastructure.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.UserId);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Password)
            .IsRequired();

        builder.Property(u => u.Salt)
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.Address)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.Latitude)
            .HasColumnType("float");

        builder.Property(u => u.Longitude)
            .HasColumnType("float");

        //  Unique constraints
        builder.HasIndex(u => u.UserName).IsUnique();  // yagona foydalanuvchi nomi
        builder.HasIndex(u => u.Email).IsUnique();     // yagona email
        builder.HasIndex(u => u.PhoneNumber).IsUnique(); // yagona telefon (agar shartli bo‘lsa ehtiyot bo‘ling)

        //  [User] -> [Role] (User.RoleId -> Role)
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        //  [User] -> [Order] (User foydalanuvchi sifatida yaratgan zakazlar)
        builder.HasMany(u => u.Orders)
            .WithOne(o => o.CreatedByUser)
            .HasForeignKey(o => o.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        //  [User] -> [Order] (User kuryer sifatida biriktirilgan zakazlar)
        builder.HasMany(u => u.AssignedOrders)
            .WithOne(o => o.AssignedCourier)
            .HasForeignKey(o => o.AssignedCourierId)
            .OnDelete(DeleteBehavior.Restrict);

        //  [User] -> [OrderAssignment] (User kuryer sifatida barcha tarixiy biriktirishlar)
        builder.HasMany(u => u.OrderAssignments)
            .WithOne(oa => oa.Courier)
            .HasForeignKey(oa => oa.CourierId)
            .OnDelete(DeleteBehavior.Restrict);

        //  [User] -> [RefreshToken] (Userga tegishli tokenlar)
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
