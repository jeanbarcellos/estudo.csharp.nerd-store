using NerdStore.WebApp.API.Tests.Config;
using Xunit;

namespace NerdStore.WebApp.API.Tests
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public UsuarioTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }
    }
}
