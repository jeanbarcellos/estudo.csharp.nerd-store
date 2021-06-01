using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AplicarVoucherPedidoCommandTests
    {
        [Fact(DisplayName = "Aplicar Voucher no Pedido Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AplicarVoucherPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(Guid.NewGuid(), "PROMO-15-OFF");

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Aplicar Voucher no Pedido Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AplicarVoucherPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(Guid.Empty, String.Empty);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AplicarVoucherPedidoValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AplicarVoucherPedidoValidator.CodigoVoucherErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }

}
