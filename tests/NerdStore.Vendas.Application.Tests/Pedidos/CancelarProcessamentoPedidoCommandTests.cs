using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class CancelarProcessamentoPedidoCommandTests
    {
        [Fact(DisplayName = "Cancelar Processamento Pedido Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void CancelarProcessamentoPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Cancelar Processamento Pedido Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void CancelarProcessamentoPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(CancelarProcessamentoPedidoValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(CancelarProcessamentoPedidoValidator.IdPedidoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }

}
