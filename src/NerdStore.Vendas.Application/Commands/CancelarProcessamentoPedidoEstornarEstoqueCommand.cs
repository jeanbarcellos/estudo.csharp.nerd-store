using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class CancelarProcessamentoPedidoEstornarEstoqueCommand : Command
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }

        public CancelarProcessamentoPedidoEstornarEstoqueCommand(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }

        public override bool EhValido()
        {
            ValidationResult = new CancelarProcessamentoPedidoEstornarEstoqueValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CancelarProcessamentoPedidoEstornarEstoqueValidator : AbstractValidator<CancelarProcessamentoPedidoEstornarEstoqueCommand>
    {
        public static string IdPedidoErroMsg => "Id do pedido inválido.";
        public static string IdClienteErroMsg => "Id do cliente inválido.";

        public CancelarProcessamentoPedidoEstornarEstoqueValidator()
        {
            RuleFor(p => p.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdPedidoErroMsg);

            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdClienteErroMsg);
        }
    }
}
