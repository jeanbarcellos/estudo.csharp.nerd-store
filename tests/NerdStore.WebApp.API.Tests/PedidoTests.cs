using NerdStore.WebApp.API.Models;
using NerdStore.WebApp.API.Tests.Config;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.API.Tests
{
    [TestCaseOrderer("NerdStore.WebApp.API.Tests.Config.PriorityOrderer", "NerdStore.WebApp.API.Tests")]
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class PedidoTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;
        private Guid _produtoId;
        private string _voucherValor;


        public PedidoTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
            _produtoId = new Guid("c498acc1-b622-4507-b3ff-64084325635e");
            _voucherValor = "PROMO-15-REAIS";
        }

        [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(1)]
        [Trait("Integration", "WebApp.API - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = _produtoId,
                Quantidade = 2
            };

            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", itemInfo);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Exibir carrinho"), TestPriority(2)]
        [Trait("Integration", "WebApp.API - Pedido")]
        public async Task ExibirCarrinho_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var getResponse = await _testsFixture.Client.GetAsync("api/carrinho");

            // Assert
            getResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Atualizar item em pedido existente"), TestPriority(3)]
        [Trait("Integration", "WebApp.API - Pedido")]
        public async Task AtualizarItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = _produtoId,
                Quantidade = 4
            };

            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var putResponse = await _testsFixture.Client.PutAsJsonAsync($"api/carrinho/{_produtoId}", itemInfo);

            // Assert
            putResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Aplicar Voucher em pedido existente"), TestPriority(4)]
        [Trait("Integration", "WebApp.API - Pedido")]
        public async Task AplicarVoucher_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new VoucherViewModel
            {
                Codigo = _voucherValor
            };

            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/carrinho/aplicar-voucher", itemInfo);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(10)]
        [Trait("Integration", "WebApp.API - Pedido")]
        public async Task RemoverItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var deleteResponse = await _testsFixture.Client.DeleteAsync($"api/carrinho/{_produtoId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }

    }
}
