using AquaCaps.Application.DTOs;
using AquaCaps.Application.DTOs.Admin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Validators;

public class OrderCreateByAdminValidators: AbstractValidator<CreateOrderDto> // Buyurtma yaratish uchun validator Admin uchun
{
    public OrderCreateByAdminValidators()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Mijoz ismi talab qilinadi.")
            .MaximumLength(100).WithMessage("Mijoz ismi 100 belgidan oshmasligi kerak.");
        RuleFor(x => x.LastName)
           
            .MaximumLength(100).WithMessage("Mijoz familiyasi 100 belgidan oshmasligi kerak.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam talab qilinadi.")
            .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Telefon raqam noto'g'ri formatda bo'lishi kerak.");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Yetkazib berish manzili talab qilinadi.")
            .MaximumLength(255).WithMessage("Yetkazib berish manzili 255 belgidan oshmasligi kerak.");
        RuleFor(x => x.CapsuleCount)
            .NotEmpty().WithMessage("Yetkazib beriladigan kapsulalar soni talab qilinadi.")
            .InclusiveBetween(1, 1000).WithMessage("Yetkazib beriladigan kapsulalar soni 1 dan 1000 gacha bo'lishi kerak.");
        RuleFor(x => x.DeliveryDate)
            .NotEmpty().WithMessage("Yetkazib berish sanasi talab qilinadi.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Yetkazib berish sanasi bugundan keyin bo'lishi kerak.");

    }
}
