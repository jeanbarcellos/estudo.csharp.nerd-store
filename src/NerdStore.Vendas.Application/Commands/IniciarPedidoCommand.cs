using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class IniciarPedidoCommand : Command
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal Total { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CVVCartao { get; private set; }

        public IniciarPedidoCommand(Guid pedidoId, Guid clienteId, decimal total, string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CVVCartao = cvvCartao;
        }

        public override bool EhValido()
        {
            ValidationResult = new IniciarPedidoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class IniciarPedidoValidator : AbstractValidator<IniciarPedidoCommand>
    {
        public IniciarPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido.");

            RuleFor(p => p.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do pedido inválido.");

            RuleFor(p => p.NomeCartao)
                .NotEmpty()
                .WithMessage("O nome do cartão não foi informado.");

            RuleFor(p => p.NumeroCartao)
                .NotEmpty()
                .WithMessage("O número do cartão não foi informado.");

            RuleFor(p => p.NumeroCartao)
                .CreditCard()
                .WithMessage("Número de cartão de crédito inválido.");

            RuleFor(p => p.ExpiracaoCartao)
                .NotEmpty()
                .WithMessage("O data de expração não foi informado.");

            RuleFor(p => p.CVVCartao)
                .Length(3 ,4)
                .WithMessage("O CVV não foi preenchido corretamente.");
        }
    }
}
