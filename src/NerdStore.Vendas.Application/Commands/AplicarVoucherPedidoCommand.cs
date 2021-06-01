using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class AplicarVoucherPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public string CodigoVoucher { get; private set; }

        public AplicarVoucherPedidoCommand(Guid clienteId, string codigoVoucher)
        {
            ClienteId = clienteId;
            CodigoVoucher = codigoVoucher;
        }

        public override bool EhValido()
        {
            ValidationResult = new AplicarVoucherPedidoValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AplicarVoucherPedidoValidator : AbstractValidator<AplicarVoucherPedidoCommand>
    {
        public static string IdClienteErroMsg => "Id do cliente inválido.";
        public static string CodigoVoucherErroMsg => "O código do voucher não pode ser vazio.";

        public AplicarVoucherPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdClienteErroMsg);

            RuleFor(p => p.CodigoVoucher)
                .NotEmpty()
                .WithMessage(CodigoVoucherErroMsg);
        }
    }
}