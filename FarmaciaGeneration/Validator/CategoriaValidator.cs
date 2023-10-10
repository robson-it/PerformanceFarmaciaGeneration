using FarmaciaGeneration.Model;
using FluentValidation;

namespace FarmaciaGeneration.Validator
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator()
        {
            RuleFor(p => p.Tipo)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250);
        }
    }
}
