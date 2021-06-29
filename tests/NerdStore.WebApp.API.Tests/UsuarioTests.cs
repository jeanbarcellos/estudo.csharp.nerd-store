using NerdStore.WebApp.API.Tests.Config;

namespace NerdStore.WebApp.API.Tests
{
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _textsFixture;

        public UsuarioTests(IntegrationTestsFixture<StartupTests> textsFixture)
        {
            _textsFixture = textsFixture;
        }
    }
}
