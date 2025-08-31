using AquaCaps.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Validators;

public class OrderUpdateValidators: AbstractValidator<OrderUpdateDto>
{
    public OrderUpdateValidators()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Manzil bo'sh bo'lmasligi kerak.");
        RuleFor(x => x.OrderedCapsuleCount)
            .NotEmpty().WithMessage("Buyurtma qilingan kapsula soni bo'sh bo'lmasligi kerak.")
            .GreaterThan(0).WithMessage("Buyurtma qilingan kapsula soni 0 dan katta bo'lishi kerak.");
        RuleFor(x => x.ReturnedCapsuleCount)
            .GreaterThanOrEqualTo(0).WithMessage("Qaytarilgan kapsula soni 0 dan katta yoki teng bo'lishi kerak.")
            .When(x => x.ReturnedCapsuleCount.HasValue);
        RuleFor(x => x.DeliveryDate)
            .NotEmpty().WithMessage("Yetkazib berish sanasi bo'sh bo'lmasligi kerak.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Yetkazib berish sanasi bugundan keyin bo'lishi kerak.")
            .Must(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            .WithMessage("Yetkazib berish sanasi yakshanba yoki shanba kunlariga to'g'ri kelmasligi kerak.");
        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Eslatma 500 ta belgidan oshmasligi kerak.")
            .When(x => !string.IsNullOrEmpty(x.Note));

    }
}
