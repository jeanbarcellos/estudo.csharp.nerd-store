using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        private readonly Guid _clienteId;
        private readonly Guid _produtoId;
        private readonly Pedido _pedido;
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _pedidoHandler;

        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _pedidoHandler = _mocker.CreateInstance<PedidoCommandHandler>();

            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();

            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
        }

        #region AdicionarItemPedidoCommand tests

        [Fact(DisplayName = "Adicionar Item Novo ao Pedido Novo com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Novo ao Pedido Rascunho com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AdicionarItem_NovoItemAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, Guid.NewGuid(), "Produto Teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Existente ao Pedido Rascunho com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, _produtoId, "Produto Xpto", 2, 100);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(5));
        }

        #endregion

        #region AtualizarItemPedidoCommand tests

        [Fact(DisplayName = "Atualizar Item Existente do Pedido Existe com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AtualizarItem_ItemExistenteAoPedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AtualizarItemPedidoCommand(_clienteId, _produtoId, 5);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterItemPorPedido(_pedido.Id, _produtoId))
                .Returns(Task.FromResult(pedidoItemExistente));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Atualizar Item Existente do Pedido Inexistente com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AtualizarItem_ItemExistenteAoPedidoInexistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(_clienteId, _produtoId, 5);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.ObterItemPorPedido(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact(DisplayName = "Atualizar Item Inexistente do Pedido Existente com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AtualizarItem_ItemInexistenteApPedidoExistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(_clienteId, _produtoId, 5);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.ObterItemPorPedido(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Never);
        }

        [Fact(DisplayName = "Atualizar Item Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AtualizarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AtualizarItemPedidoCommand(Guid.Empty, Guid.Empty, 0);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(3));
        }

        #endregion

        #region RemoverItemPedidoCommand tests

        [Fact(DisplayName = "Remover Item Existente do Pedido Existe com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task RemoverItem_ItemExistenteAoPedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new RemoverItemPedidoCommand(_clienteId, _produtoId);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterItemPorPedido(_pedido.Id, _produtoId))
                .Returns(Task.FromResult(pedidoItemExistente));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.RemoverItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Remover Item Existente do Pedido Inexistente com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task RemoverItem_ItemExistenteAoPedidoInexistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new RemoverItemPedidoCommand(_clienteId, _produtoId);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.ObterItemPorPedido(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact(DisplayName = "Remover Item Inexistente do Pedido Existente com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task RemoverItem_ItemInexistenteAoPedidoExistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new RemoverItemPedidoCommand(_clienteId, _produtoId);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterItemPorPedido(_pedido.Id, _produtoId))
                .Returns(Task.FromResult(new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100)));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.ObterItemPorPedido(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.RemoverItem(It.IsAny<PedidoItem>()), Times.Never);
        }

        #endregion
    }
}
