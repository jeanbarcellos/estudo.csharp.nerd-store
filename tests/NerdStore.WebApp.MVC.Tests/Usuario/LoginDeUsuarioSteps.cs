using NerdStore.WebApp.MVC.Tests.Config;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.WebApp.MVC.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class LoginDeUsuarioSteps
    {
        private readonly LoginDeUsuarioTela _loginUsuarioTela;
        private readonly AutomacaoWebTestsFixture _testsFixture;

        public LoginDeUsuarioSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _loginUsuarioTela = new LoginDeUsuarioTela(testsFixture.BrowserHelper);
        }

        [When(@"Ele clicar em login")]
        public void QuandoEleClicarEmLogin()
        {
            // Act
            _loginUsuarioTela.ClicarNoLinkLogin();

            // Assert
            Assert.Contains(_testsFixture.Configuration.LoginUrl,
                _loginUsuarioTela.ObterUrl());
        }


        [When(@"Preencher os dados do formulario de login")]
        public void QuandoPreencherOsDadosDoFormularioDeLogin(Table table)
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "teste@teste.com",
                Senha = "Teste@123"
            };
            _testsFixture.Usuario = usuario;

            // Act
            _loginUsuarioTela.PreencherFormularioLogin(usuario);

            // Assert
            Assert.True(_loginUsuarioTela.ValidarPreenchimentoFormularioLogin(usuario));
        }

        [When(@"Clicar no botão login")]
        public void QuandoClicarNoBotaoLogin()
        {
            _loginUsuarioTela.ClicarNoBotaoLogin();
        }
    }
}
