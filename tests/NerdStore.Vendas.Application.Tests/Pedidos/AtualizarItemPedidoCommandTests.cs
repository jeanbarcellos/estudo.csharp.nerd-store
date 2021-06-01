using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AtualizarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Atualizar Item Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AtualizarItemPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), 3);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Atualizar Item Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AtualizarItemPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.Empty, Guid.Empty, 0);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AtualizarItemPedidoValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AtualizarItemPedidoValidator.IdProdutoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AtualizarItemPedidoValidator.QtdMinErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Atualizar Item Command unidades acima do permitido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AtualizarItemPedidoCommand_QuantidadeUnidadesSuperiorAoPermitido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), Pedido.MAX_UNIDADES_ITEM + 1);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AtualizarItemPedidoValidator.QtdMaxErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }

}
