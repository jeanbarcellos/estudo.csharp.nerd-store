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
        public static string IdClienteErroMsg => "Id do cliente inválido.";
        public static string IdPedidoErroMsg => "Id do pedido inválido.";
        public static string NomeCartaoErroMsg => "O nome do cartão não foi informado.";
        public static string NumeroCartaoNotEmptyMsg => "O número do cartão não foi informado.";
        public static string NumeroCartaoErroMsg => "Número de cartão de crédito inválido.";
        public static string ExpiracaoCartaoErroMsg => "O data de exipração não foi informado.";
        public static string CVVCartaoErroMsg => "O CVV não foi preenchido corretamente.";

        public IniciarPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdClienteErroMsg);

            RuleFor(p => p.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdPedidoErroMsg);

            RuleFor(p => p.NomeCartao)
                .NotEmpty()
                .WithMessage(NomeCartaoErroMsg);

            RuleFor(p => p.NumeroCartao)
                .NotEmpty()
                .WithMessage(NumeroCartaoNotEmptyMsg);

            RuleFor(p => p.NumeroCartao)
                .CreditCard()
                .WithMessage(NumeroCartaoErroMsg);

            RuleFor(p => p.ExpiracaoCartao)
                .NotEmpty()
                .WithMessage(ExpiracaoCartaoErroMsg);

            RuleFor(p => p.CVVCartao)
                .Length(3 ,4)
                .WithMessage(CVVCartaoErroMsg);
        }
    }
}
