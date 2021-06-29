using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.API.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.API.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>> { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly LojaAppFactory<TStartup> Factory;
        public HttpClient Client;

        public string UsuarioToken;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new LojaAppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public async Task RealizarLogin()
        {
            var userData = new LoginViewModel
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };

            // Recriando o client para evitar configurações de Web
            Client = Factory.CreateClient();

            var response = await Client.PostAsJsonAsync("api/login", userData);
            response.EnsureSuccessStatusCode();

            UsuarioToken = await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
