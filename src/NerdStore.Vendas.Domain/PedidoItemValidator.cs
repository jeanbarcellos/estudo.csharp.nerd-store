using FluentValidation;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItemValidator : AbstractValidator<PedidoItem>
    {
        public static string ProdutoIdNotEmptyMessage => "O ID do produto precisa ser informado";
        public static string ProdutoNomeNotEmptyMessage => "O nome do produto precisa ser";
        public static string QuantidadeErrorMessage => $"Mínimo de {Pedido.MIN_UNIDADES_ITEM} unidades por produto";
        public static string ValorUnitarioErrorMessage => "O valor unitário precisa ser superior a 0";

        public PedidoItemValidator()
        {
            RuleFor(c => c.ProdutoId)
                .NotEmpty().WithMessage(ProdutoIdNotEmptyMessage);

            RuleFor(c => c.ProdutoNome)
                .NotEmpty().WithMessage(ProdutoNomeNotEmptyMessage);

            RuleFor(f => f.Quantidade)
                .NotNull()
                .WithMessage(QuantidadeErrorMessage)
                .GreaterThanOrEqualTo(Pedido.MIN_UNIDADES_ITEM)
                .WithMessage(QuantidadeErrorMessage);

            RuleFor(f => f.ValorUnitario)
                .NotNull()
                .WithMessage(ValorUnitarioErrorMessage)
                .GreaterThan(0)
                .WithMessage(ValorUnitarioErrorMessage);
        }
    }
}
