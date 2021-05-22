
using NerdStore.Core.DomainObjects;
using Xunit;

namespace NerdStore.Catalogo.Domain.Tests
{
    public class CategoriaTests
    {
        [Fact(DisplayName = "Nova Categoria")]
        [Trait("Unit", "Catalogo.Domain - Categoria")]
        public void Categoria_NovaCategoria_CategoriaValida()
        {
            // Arrange
            var nome = "Categoria 01";
            var codigo = 1001;

            // Act
            var result = new Categoria(nome, codigo);

            // Assert
            Assert.Equal(nome, result.Nome);
            Assert.Equal(codigo, result.Codigo);
        }

        [Fact(DisplayName = "Nova Categoria - Lançar exceptions")]
        [Trait("Unit", "Catalogo.Domain - Categoria")]
        public void Categoria_NovaCategoria_ValidacoesDevemLancarExeptions()
        {
            // Arrange
            var nome = string.Empty;
            var codigo = 0;

            // Act && Assert
            var ex = Assert.Throws<DomainException>(() =>
            {
                new Categoria(nome, codigo);
            });
            Assert.Equal("O campo Nome da Categoria não pode estar vazio.", ex.Message);
        }

    }
}
