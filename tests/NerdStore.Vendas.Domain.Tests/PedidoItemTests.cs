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
            var produtoId = Guid.NewGuid();
            var produtoNome = "Produto Teste";
            var quantidade = Pedido.MIN_UNIDADES_ITEM + 1;
            var valorUnitario = 10;
            var pedidoItem = new PedidoItem(produtoId, produtoNome, quantidade, valorUnitario);

            // Act
            var result = pedidoItem.EhValido();

            // Assert
            Assert.True(result);
            Assert.Equal(produtoId, pedidoItem.ProdutoId);
            Assert.Equal(produtoNome, pedidoItem.ProdutoNome);
            Assert.Equal(quantidade, pedidoItem.Quantidade);
            Assert.Equal(valorUnitario, pedidoItem.ValorUnitario);
        }

        [Fact(DisplayName = "Validar PedidoItem que deverá estar inválido")]
        [Trait("Unit", "Vendas.Domain - PedidoItem")]
        public void PedidoItem_ValidarPedidoItem_DeveEstarInvalido()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.Empty, "", Pedido.MIN_UNIDADES_ITEM, 0);

            // Act
            var result = pedidoItem.EhValido();

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Noto Item Pedido com unidades abaixo do permitido")]
        [Trait("Unit", "Vendas.Domain - PedidoItem")]
        public void AdicionarItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));
            Assert.Equal(PedidoItemValidator.QuantidadeErrorMessage, ex.Message);
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
