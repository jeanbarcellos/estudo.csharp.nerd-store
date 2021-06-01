using System;
using FluentValidation;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands
{
    public class AtualizarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }


        public AtualizarItemPedidoCommand(Guid clienteId, Guid produtoId, int quantidade)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public override bool EhValido()
        {
            ValidationResult = new AtualizarItemPedidoValidator().Validate(this);
            return ValidationResult.IsValid;
        }

    }

    public class AtualizarItemPedidoValidator : AbstractValidator<AtualizarItemPedidoCommand>
    {
        public static string IdClienteErroMsg => "Id do cliente inválido";
        public static string IdProdutoErroMsg => "Id do produto inválido";
        public static string QtdMinErroMsg => $"A quantidade miníma de um item é {Pedido.MIN_UNIDADES_ITEM}";
        public static string QtdMaxErroMsg => $"A quantidade máxima de um item é {Pedido.MAX_UNIDADES_ITEM}";

        public AtualizarItemPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdClienteErroMsg);

            RuleFor(p => p.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdProdutoErroMsg);

            RuleFor(c => c.Quantidade)
                .GreaterThanOrEqualTo(Pedido.MIN_UNIDADES_ITEM)
                .WithMessage(QtdMinErroMsg);

            RuleFor(c => c.Quantidade)
                .LessThanOrEqualTo(Pedido.MAX_UNIDADES_ITEM)
                .WithMessage(QtdMaxErroMsg);
        }
    }
}
