using NerdStore.Catalogo.Application.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace NerdStore.Catalogo.Application.Tests
{
    public class CategoriaViewModelTests
    {
        [Fact(DisplayName = "CategoriaViewModel V�lido")]
        [Trait("Unit", "Catalogo.Application - CategoriaViewModel")]
        public void CategoriaViewModel_Novo_Valido()
        {
            // Arrange
            var categoriaViewModel = new CategoriaViewModel
            {
                Nome = "Nome",
                Codigo = 987
            };

            // Act
            var result = ValidateModel(categoriaViewModel);

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact(DisplayName = "CategoriaViewModel Inv�lido")]
        [Trait("Unit", "Catalogo.Application - CategoriaViewModel")]
        public void CategoriaViewModel_Novo_Invalido()
        {
            // Arrange
            var categoriaViewModel = new CategoriaViewModel();

            // Act
            var result = ValidateModel(categoriaViewModel);

            // Assert
            Assert.True(result.Count > 0);
            Assert.Contains("O campo Nome � obrigat�rio.", result.Select(c => c.ErrorMessage));
            Assert.Contains("O campo Codigo precisa ter o valor m�nimo de 1.", result.Select(c => c.ErrorMessage));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
