using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class RemoverItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }


        public RemoverItemPedidoCommand(Guid clienteId, Guid produtoId)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
        }

        public override bool EhValido()
        {
            ValidationResult = new RemoverItemPedidoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoverItemPedidoValidator : AbstractValidator<RemoverItemPedidoCommand>
    {
        public RemoverItemPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido.");

            RuleFor(p => p.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do produto inválido.");
        }
    }
}
