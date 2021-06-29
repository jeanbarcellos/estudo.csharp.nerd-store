using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace NerdStore.WebApp.API.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationTestsFictureCollection))]
    public class IntegrationTestsFictureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTests>> { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly LojaAppFactory<TStartup> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {

            };

            Factory = new LojaAppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
