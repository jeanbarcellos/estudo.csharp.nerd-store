using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class FinalizarPedidoCommandTests
    {
        [Fact(DisplayName = "Finalizar Pedido Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void FinalizarPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new FinalizarPedidoCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Finalizar Pedido Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void FinalizarPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new FinalizarPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(FinalizarPedidoValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(FinalizarPedidoValidator.IdPedidoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }

}
