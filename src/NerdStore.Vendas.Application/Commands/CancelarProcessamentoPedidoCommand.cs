using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class CancelarProcessamentoPedidoCommand : Command
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }


        public CancelarProcessamentoPedidoCommand(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }

        public override bool EhValido()
        {
            ValidationResult = new CancelarProcessamentoPedidoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CancelarProcessamentoPedidoValidator : AbstractValidator<CancelarProcessamentoPedidoCommand>
    {
        public static string IdClienteErroMsg => "Id do cliente inválido.";
        public static string IdPedidoErroMsg => "Id do pedido inválido.";

        public CancelarProcessamentoPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdClienteErroMsg);

            RuleFor(p => p.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdPedidoErroMsg);
        }
    }
}
