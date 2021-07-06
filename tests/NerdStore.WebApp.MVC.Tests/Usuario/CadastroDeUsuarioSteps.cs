using System;
using TechTalk.SpecFlow;

namespace NerdStore.WebApp.MVC.Tests.Usuario
{
    [Binding]
    public class CadastroDeUsuarioSteps
    {
        [When(@"Ele clicar em registrar")]
        public void QuandoEleClicarEmRegistrar()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Preencher os dados do formulario")]
        public void QuandoPreencherOsDadosDoFormulario(Table table)
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Clicar no botão registrar")]
        public void QuandoClicarNoBotaoRegistrar()
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Preencher os dados do formulario com uma senha sem maiusculas")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemMaiusculas(Table table)
        {
            // Arrange

            // Act

            // Assert
        }

        [When(@"Preencher os dados do formulario com uma senha sem caractere especial")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemCaractereEspecial(Table table)
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiuscula")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmaLetraMaiuscula()
        {
            // Arrange

            // Act

            // Assert
        }

        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmCaractereEspecial()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
