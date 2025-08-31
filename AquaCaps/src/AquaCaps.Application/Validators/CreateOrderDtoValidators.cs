using AquaCaps.Application.DTOs.Admin;
using FluentValidation;

namespace AquaCaps.Application.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Mijoz ismi talab qilinadi.")
            .Length(2, 50).WithMessage("Mijoz ismi 2 dan 50 ta belgigacha bo'lishi kerak.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Mijoz familiyasi talab qilinadi.")
            .Length(2, 50).WithMessage("Mijoz familiyasi 2 dan 50 ta belgigacha bo'lishi kerak.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam talab qilinadi.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Telefon raqam noto'g'ri formatda. Masalan: +998901234567");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Yetkazib berish manzili talab qilinadi.")
            .Length(10, 200).WithMessage("Yetkazib berish manzili 10 dan 200 ta belgigacha bo'lishi kerak.");

        RuleFor(x => x.CapsuleCount)
            .NotEmpty().WithMessage("Yetkazib beriladigan kapsulalar soni talab qilinadi.")
            .InclusiveBetween(1, 200).WithMessage("Yetkazib beriladigan kapsulalar soni 1 dan 200 gacha bo'lishi kerak.");

        RuleFor(x => x.DeliveryDate)
            .NotEmpty().WithMessage("Yetkazib berish sanasi talab qilinadi.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Yetkazib berish sanasi bugundan keyin bo'lishi kerak.")
            .Must(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            .WithMessage("Yetkazib berish sanasi yakshanba yoki shanba kunlariga to'g'ri kelmasligi kerak.");
    }
}
