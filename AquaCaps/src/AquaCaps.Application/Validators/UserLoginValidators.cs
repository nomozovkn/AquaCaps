using AquaCaps.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Validators;

public class UserLoginValidators : AbstractValidator<UserLogInDto>
{
    public UserLoginValidators()
    {
        RuleFor(x => x.UserName)
          .NotEmpty()
          .WithMessage("UserName is required")
          .Length(3, 20)
          .WithMessage("UserName must be between 3 and 20 characters long");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Length(6, 20)
            .WithMessage("Password must be between 8 and 20 characters long");
    }
}
