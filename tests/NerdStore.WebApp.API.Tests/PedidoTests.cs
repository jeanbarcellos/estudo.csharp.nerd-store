using NerdStore.WebApp.API.Models;
using NerdStore.WebApp.API.Tests.Config;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.API.Tests
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class PedidoTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public PedidoTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(1)]
        [Trait("Integration", "WebApp.API - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveRetornarComSucesso()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("c498acc1-b622-4507-b3ff-64084325635e"),
                Quantidade = 2
            };

            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", itemInfo);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

    }
}
