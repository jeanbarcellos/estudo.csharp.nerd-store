using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AdicionarItemPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Produto Teste", 2, 100);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AdicionarItemPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty,
                Guid.Empty, "", 0, 0);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AdicionarItemPedidoValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.IdProdutoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.NomeErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.QtdMinErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.ValorErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Adicionar Item Command unidades acima do permitido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void AdicionarItemPedidoCommand_QuantidadeUnidadesSuperiorAoPermitido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AdicionarItemPedidoValidator.QtdMaxErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }

}
