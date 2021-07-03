using NerdStore.WebApp.API.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.API.Tests
{
    [TestCaseOrderer("NerdStore.WebApp.API.Tests.Config.PriorityOrderer", "NerdStore.WebApp.API.Tests")]
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class VitrineTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public VitrineTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Exibir lista de produtos"), TestPriority(1)]
        [Trait("Integration", "WebApp.API - Vitrine")]
        public async Task ExibirLista_VitrineExistente_DeveRetornarComSucesso()
        {
            // Arrange
            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var getResponse = await _testsFixture.Client.GetAsync("api/vitrine");

            // Assert
            getResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Exibir detalhes de um produto"), TestPriority(1)]
        [Trait("Integration", "WebApp.API - Vitrine")]
        public async Task ExibirDetalhesProduto_VitrineExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("c498acc1-b622-4507-b3ff-64084325635e");

            await _testsFixture.RealizarLogin();
            _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);

            // Act
            var getResponse = await _testsFixture.Client.GetAsync($"api/vitrine/{produtoId}");

            // Assert
            getResponse.EnsureSuccessStatusCode();
        }



    }
}
