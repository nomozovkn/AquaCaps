using AquaCaps.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Validators;

public class OrderCreateValidators: AbstractValidator<OrderCreateDto> // Buyurtma yaratish uchun validator Client uchun
{
    public OrderCreateValidators()
    {
        RuleFor(x => x.OrderedCapsuleCount)
            .NotEmpty().WithMessage("Buyurtma qilingan kapsula soni talab qilinadi.")
            .InclusiveBetween(1, 200).WithMessage("Buyurtma qilingan kapsula soni 1 dan 200 gacha bo'lishi kerak.");
        RuleFor(x => x.DeliveryDate)
            .NotEmpty().WithMessage("Yetkazib berish sanasi talab qilinadi.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Yetkazib berish sanasi bugundan keyin bo'lishi kerak.");
        RuleFor(x => x.ReturnedCapsuleCount)
            .InclusiveBetween(0, 200).WithMessage("Qaytarilgan kapsula soni 0 dan 1000 gacha bo'lishi kerak.")
            .When(x => x.ReturnedCapsuleCount.HasValue);
    }
}
