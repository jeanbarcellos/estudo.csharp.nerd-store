using System;
using FluentValidation;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Commands
{
    public class FinalizarPedidoCommand : Command
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }

        public FinalizarPedidoCommand(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }

        public override bool EhValido()
        {
            ValidationResult = new FinalizarPedidoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class FinalizarPedidoValidator : AbstractValidator<FinalizarPedidoCommand>
    {
        public FinalizarPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido.");

            RuleFor(p => p.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do pedido inválido.");
        }

        private object RuleFor(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
