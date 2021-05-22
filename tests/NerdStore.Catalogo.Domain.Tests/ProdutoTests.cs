using Xunit;
using System;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain.Tests
{
    public class ProdutoTests
    {
        private Produto _produto;

        public ProdutoTests()
        {
            _produto = new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1));
        }

        [Fact(DisplayName = "Novo Produto")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_NovoProduto_ProdutoValido()
        {
            // Arrange
            var nome = "Nome";
            var descricao = "Descricao";
            var ativo = false;
            var valor = 100;
            var categoriaId = Guid.NewGuid();
            var dataCadastro = DateTime.Now;
            var imagem = "Imagem";
            var dimensoes = new Dimensoes(1, 1, 1);

            // Act
            var result = new Produto(nome, descricao, ativo, valor, categoriaId, dataCadastro, imagem, dimensoes);

            // Assert
            Assert.Equal(nome, result.Nome);
            Assert.Equal(descricao, result.Descricao);
            Assert.Equal(ativo, result.Ativo);
            Assert.Equal(valor, result.Valor);
            Assert.Equal(categoriaId, result.CategoriaId);
            Assert.Equal(dataCadastro, result.DataCadastro);
            Assert.Equal(imagem, result.Imagem);
            Assert.Equal(dimensoes, result.Dimensoes);
        }

        [Fact(DisplayName = "Validar Produto")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_Validar_ProdutoValido()
        {
            var ex = Record.Exception(() =>
                _produto.Validar()
            );

            Assert.Null(ex);
        }

        [Fact(DisplayName = "Validar Produto - Lança exceptions")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_Validar_ValidacoesDevemLancarExceptions()
        {
            var ex = Assert.Throws<DomainException>(() =>
                new Produto(string.Empty, "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );
            Assert.Equal("O campo Nome do Produto não pode ser vazio.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", string.Empty, false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );
            Assert.Equal("O campo Descricao do Produto não pode ser vazio.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 0, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );
            Assert.Equal("O campo Valor do Produto não pode ser menor igual a 0.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.Empty, DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
            );
            Assert.Equal("O campo CategoriaId do Produto não pode estar vazio.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, string.Empty, new Dimensoes(1, 1, 1))
            );
            Assert.Equal("O campo Imagem do Produto não pode estar vazio.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(0, 1, 1))
            );
            Assert.Equal("O campo Altura não pode ser menor ou igual a 0.", ex.Message);
        }

        [Fact(DisplayName = "Ativar Produto")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_Ativar_DeveAtivarProduto()
        {
            // Arrange
            var produto = _produto;

            // Act
            produto.Ativar();

            // Assert
            Assert.True(produto.Ativo);
        }

        [Fact(DisplayName = "Desativar Produto")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_Desativar_DeveDesativarProduto()
        {
            // Arrange
            var produto = new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1));

            // Act
            produto.Desativar();

            // Assert
            Assert.False(produto.Ativo);
        }

        [Fact(DisplayName = "Alterar categoria do Produto")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_AlterarCategoria_DeveAlterarCategoria()
        {
            // Arrange
            var categoria = new Categoria("Categoria X", 999);
            var produto = _produto;

            // Act
            produto.AlterarCategoria(categoria);

            // Assert
            Assert.Equal(categoria, produto.Categoria);
            Assert.Equal(categoria.Id, produto.CategoriaId);
        }

        [Fact(DisplayName = "Alterar descrição do Produto")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_AlterarDescricao_DeveAlterarDescricao()
        {
            // Arrange
            var produto = _produto;

            // Act
            produto.AlterarDescricao("Descricao 456");

            // Assert
            Assert.Equal("Descricao 456", produto.Descricao);
        }

        [Fact(DisplayName = "Alterar descrição do Produto - Lança exception")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_AlterarDescricao_ValidacaoDevemLancarException()
        {
            // Arrange
            var produto = _produto;

            // Act && Assert
            var ex = Assert.Throws<DomainException>(() =>
                produto.AlterarDescricao(string.Empty)
            );

            Assert.Equal("O campo Descricao do Produto não pode ser vazio.", ex.Message);
        }

        [Fact(DisplayName = "Repor estoque")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_ReporEstoque_DeveIncrementarEstoque()
        {
            // Arrange
            var produto = _produto;

            // Act
            produto.ReporEstoque(159);

            // Assert
            Assert.Equal(159, produto.QuantidadeEstoque);
        }

        [Fact(DisplayName = "Verificar se possui estoque - Deve existir")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_PosuiEstoque_DeveRetornarTrue()
        {
            // Arrange
            var produto = _produto;
            produto.ReporEstoque(100);

            // Act
            var result = produto.PossuiEstoque(5);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Verificar se possui estoque - Não deve existir")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_PosuiEstoque_DeveRetornarFalse()
        {
            // Arrange
            var produto = _produto;

            // Act
            var result = produto.PossuiEstoque(5);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Debitar estoque")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_DebitarEstoque_DeveDecrementarEstoque()
        {
            // Arrange
            var produto = _produto;
            produto.ReporEstoque(100);

            // Act
            produto.DebitarEstoque(10);

            // Assert
            Assert.Equal(90, produto.QuantidadeEstoque);
        }

        [Fact(DisplayName = "Debitar estoque - Lança exception")]
        [Trait("Unit", "Catalogo.Domain - Produto")]
        public void Produto_DebitarEstoque_DeveLançarException()
        {
            // Arrange
            var produto = _produto;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => produto.DebitarEstoque(10));
            Assert.Equal("Estoque insuficiente.", ex.Message);
        }
    }
}