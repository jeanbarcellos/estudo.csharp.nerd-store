using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class CancelarProcessamentoPedidoEstornarEstoqueCommandTests
    {
        [Fact(DisplayName = "Cancelar Processamento Pedido e Estornar Estoque Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void CancelarProcessamentoPedidoEstornarEstoqueCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoEstornarEstoqueCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Cancelar Processamento Pedido e Estornar Estoque Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void CancelarProcessamentoPedidoEstornarEstoqueCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoEstornarEstoqueCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(CancelarProcessamentoPedidoEstornarEstoqueValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(CancelarProcessamentoPedidoEstornarEstoqueValidator.IdPedidoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }

}
