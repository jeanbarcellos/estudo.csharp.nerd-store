using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Catalogo.Domain.Tests
{
    public class DimensoesTests
    {
        [Fact(DisplayName = "Nova Dimensao válida")]
        [Trait("Unit", "Catalogo.Domain - Dimensoes")]
        public void Dimensoes_Nova_DimensaoValida()
        {
            // Arrange & Act
            var result = new Dimensoes(30, 20, 10);

            // Assert
            Assert.Equal(30, result.Altura);
            Assert.Equal(20, result.Largura);
            Assert.Equal(10, result.Profundidade);
        }

        [Fact(DisplayName = "Nova Dimensao inválida")]
        [Trait("Unit", "Catalogo.Domain - Dimensoes")]
        public void Dimensoes_Nova_ValidacoesDeveLancarExceptions()
        {
            // Arrange & Act && Assert
            var ex = Assert.Throws<DomainException>(() => new Dimensoes(0, 0, 0));
            Assert.Equal("O campo Altura não pode ser menor ou igual a 0.", ex.Message);
        }

        [Fact(DisplayName = "Descricao Formatada")]
        [Trait("Unit", "Catalogo.Domain - Dimensoes")]
        public void Dimensoes_DescricaoFormatada_DescricaoFormatada()
        {
            // Arrange
            var dimensoes = new Dimensoes(30, 20, 10);

            // Act
            var result = dimensoes.DescricaoFormatada();

            // Assert
            Assert.Equal("LxAxP: 20 x 30 x 10", result);
        }

        [Fact(DisplayName = "ToString")]
        [Trait("Unit", "Catalogo.Domain - Dimensoes")]
        public void Dimensoes_ToString_DescricaoFormatada()
        {
            // Arrange
            var dimensoes = new Dimensoes(30, 20, 10);

            // Act
            var result = dimensoes.ToString();

            // Assert
            Assert.Equal("LxAxP: 20 x 30 x 10", result);
        }
    }
}
