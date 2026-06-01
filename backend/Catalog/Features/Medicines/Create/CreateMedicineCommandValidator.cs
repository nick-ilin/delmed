using FluentValidation;

namespace Catalog.Features.Medicines.Create;

public class CreateMedicineCommandValidator : AbstractValidator<CreateMedicineCommand>
{
    public CreateMedicineCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название обязательно")
            .MaximumLength(200);

        RuleFor(x => x.Manufacturer)
            .NotEmpty().WithMessage("Производитель обязателен")
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0");
    }
}