using NerdStore.Core.DomainObjects;
using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Validar PedidoItem que deverá estar Válido")]
        [Trait("Unit", "Vendas.Domain - PedidoItem")]
        public void PedidoItem_ValidarPedidoItem_DeveEstarValido()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM + 1, 100);

            // Act
            var result = pedidoItem.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Noto Item Pedido com unidades abaixo do permitido")]
        [Trait("Unit", "Vendas.Domain - PedidoItem")]
        public void AdicionarItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));
        }

        [Fact(DisplayName = "Calcular o valor do Item Pedido")]
        [Trait("Unit", "Vendas.Domain - PedidoItem")]
        public void CalcularValor_PedidoComItens_DeveRetonarValor()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

            // Act
            var result = pedidoItem.CalcularValor();

            // Assert
            Assert.Equal(500.00m, result);
        }
    }
}
