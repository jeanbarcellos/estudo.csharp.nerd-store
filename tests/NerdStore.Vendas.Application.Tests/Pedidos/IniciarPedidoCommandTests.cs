using NerdStore.Vendas.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class IniciarPedidoCommandTests
    {
        [Fact(DisplayName = "Iniciar Pedido Command Válido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void IniciarPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new IniciarPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), 100.99m,
                "Nome do Cartão", "1111-2222-3333-4444", "10/23","123");

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Iniciar Pedido Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void IniciarPedidoCommand_CommandoEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new IniciarPedidoCommand(Guid.Empty, Guid.Empty, 0, "", "", "", "");

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(IniciarPedidoValidator.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(IniciarPedidoValidator.IdPedidoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(IniciarPedidoValidator.NomeCartaoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(IniciarPedidoValidator.NumeroCartaoNotEmptyMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(IniciarPedidoValidator.ExpiracaoCartaoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(IniciarPedidoValidator.CVVCartaoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Iniciar Pedido Command com Número do Cartão Iválido")]
        [Trait("Unit", "Vendas.Application - Pedido Commands")]
        public void IniciarPedidoCommand_CommandoComNumeroDoCartaoInvaValido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new IniciarPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), 100.99m,
                "Nome do Cartão", "111-2222-333-444", "10/23", "123");

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(IniciarPedidoValidator.NumeroCartaoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

    }

}
