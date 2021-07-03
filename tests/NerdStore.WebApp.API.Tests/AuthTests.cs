using NerdStore.WebApp.API.Models;
using NerdStore.WebApp.API.Tests.Config;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.API.Tests
{
    [TestCaseOrderer("NerdStore.WebApp.API.Tests.Config.PriorityOrderer", "NerdStore.WebApp.API.Tests")]
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class AuthTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public AuthTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Login no sistema com sucesso"), TestPriority(1)]
        [Trait("Integration", "WebApp.API - Auth")]
        public async Task Login_NovoAcesso_DeveRetornarComSucesso()
        {
            // Arrange
            var userData = new LoginViewModel
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/login", userData);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }


        [Fact(DisplayName = "Login no sistema com erro"), TestPriority(2)]
        [Trait("Integration", "WebApp.API - Auth")]
        public async Task Login_NovoAcesso_DeveRetornarErro()
        {
            // Arrange
            var userData = new LoginViewModel
            {
                Email = "2423423@teste.com",
                Senha = "243234@123"
            };

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/login", userData);

            // Assert
            //postResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }
    }
}
