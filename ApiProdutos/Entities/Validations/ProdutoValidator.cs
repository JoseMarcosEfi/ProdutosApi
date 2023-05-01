using FluentValidation;

namespace ApiProdutos.Entities.Validations
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório")
                .Length(3, 100).WithMessage("O nome do produto deve ter entre 3 e 100 caracteres");

            RuleFor(p => p.Preco)
                .NotEmpty().WithMessage("O preço do produto é obrigatório.")
                .GreaterThanOrEqualTo(0).WithMessage("O preço do produto deve ser maior ou igual a zero.");
        }
    }
    
}
