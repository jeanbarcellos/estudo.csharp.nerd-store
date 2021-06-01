using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class RemoverItemPedidoCommandTests
    {
        [Fact(DisplayName = "Remover Item Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void RemoverItemPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new RemoverItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Remover Item Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void RemoverItemPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new RemoverItemPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(RemoverItemPedidoValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(RemoverItemPedidoValidator.IdProdutoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }

}
