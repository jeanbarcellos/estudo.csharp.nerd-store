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
        private readonly Voucher _voucherTipoValor;

        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _pedidoHandler = _mocker.CreateInstance<PedidoCommandHandler>();

            _clienteId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();

            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);

            _voucherTipoValor = new Voucher("PROMO-15-REAIS", null, 15, 10, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);
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

        [Fact(DisplayName = "Atualizar Item Existente do Pedido Existente com Sucesso")]
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

        [Fact(DisplayName = "Atualizar Item Existente do Pedido Inexistente sem Sucesso")]
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

        [Fact(DisplayName = "Atualizar Item Inexistente do Pedido Existente sem Sucesso")]
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

        [Fact(DisplayName = "Remover Item Existente do Pedido Existente com Sucesso")]
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

        [Fact(DisplayName = "Remover Item Existente do Pedido Inexistente sem Sucesso")]
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

        [Fact(DisplayName = "Remover Item Inexistente do Pedido Existente sem Sucesso")]
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

        [Fact(DisplayName = "Remover Item Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task RemoverItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new RemoverItemPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(2));
        }

        #endregion

        #region AplicarVoucherPedidoCommand tests

        [Fact(DisplayName = "Aplicar Voucher Existente ao Pedido Existente com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AplicarVoucher_VoucherExistenteAoPedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, _voucherTipoValor.Codigo);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterVoucherPorCodigo(_voucherTipoValor.Codigo))
                .Returns(Task.FromResult(_voucherTipoValor));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact(DisplayName = "Aplicar Voucher Existente ao do Pedido Inexistente sem Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AplicarVoucher_VoucherExistenteAoPedidoInexistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, _voucherTipoValor.Codigo);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.ObterVoucherPorCodigo(It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "Aplicar Voucher inexistente ao Pedido Existente sem Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AplicarVoucher_VoucherInexistenteAoPedidoExistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, _voucherTipoValor.Codigo);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Never);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Aplicar Voucher expirado ao Pedido Existente sem Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AplicarVoucher_VoucherExpiradoAoPedidoExistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var voucherInvalido = new Voucher("PROMO-15-REAIS", null, null, 0, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);
            var pedidoCommand = new AplicarVoucherPedidoCommand(_clienteId, voucherInvalido.Codigo);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterVoucherPorCodigo(voucherInvalido.Codigo))
                .Returns(Task.FromResult(voucherInvalido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(5));
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Never);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Aplicar Voucher Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task AplicarVoucher_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AplicarVoucherPedidoCommand(Guid.Empty, "");

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(2));
        }

        #endregion

        #region IniciarPedidoCommand tests

        [Fact(DisplayName = "Iniciar Pedido com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task IniciarPedido_PedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new IniciarPedidoCommand(_pedido.Id, _clienteId, 100.99m, "Nome do Cartão", "1111-2222-3333-4444", "10/23", "123");

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
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Iniciar Pedido Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task IniciarPedido_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new IniciarPedidoCommand(Guid.Empty, Guid.Empty, 0, "", "", "", "");

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId))
                .Returns(Task.FromResult(_pedido));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(6));
        }

        #endregion

        #region FinalizarPedidoCommand tests

        [Fact(DisplayName = "Finalizar Pedido Existente com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task FinalizarPedido_PedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new FinalizarPedidoCommand(_pedido.Id, _clienteId);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPorId(_pedido.Id))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Finalizar Pedido Inexistente sem Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task FinalizarPedido_PedidoInexistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new FinalizarPedidoCommand(_pedido.Id, _clienteId);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Finalizar Pedido Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task FinalizarPedido_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new FinalizarPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(2));
        }

        #endregion

        #region CancelarProcessamentoPedidoEstornarEstoqueCommand tests

        [Fact(DisplayName = "Cancelar Processamento do Pedido e Estornar Estoque com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task CancelarProcessamentoPedidoEstornarEstoque_PedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new CancelarProcessamentoPedidoEstornarEstoqueCommand(_pedido.Id, _clienteId);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPorId(_pedido.Id))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Cancelar Processamento do Pedido e Estornar Estoque sem Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task CancelarProcessamentoPedidoEstornarEstoque_PedidoInexistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoEstornarEstoqueCommand(_pedido.Id, _clienteId);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Cancelar Processamento do Pedido e Estornar Estoque Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task CancelarProcessamentoPedidoEstornarEstoque_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoEstornarEstoqueCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(2));
        }

        #endregion

        #region CancelarProcessamentoPedidoCommand tests

        [Fact(DisplayName = "Cancelar Processamento do Pedido com Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task CancelarProcessamentoPedido_PedidoExistente_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new CancelarProcessamentoPedidoCommand(_pedido.Id, _clienteId);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPorId(_pedido.Id))
                .Returns(Task.FromResult(_pedido));
            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Cancelar Processamento do Pedido sem Sucesso")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task CancelarProcessamentoPedido_PedidoInexistente_DeveOcorrerErroELancarNotificacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoCommand(_pedido.Id, _clienteId);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Cancelar Processamento do Pedido Command Inválido")]
        [Trait("Unit", "Vendas.Application - Pedido Command Handler")]
        public async Task CancelarProcessamentoPedido_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new CancelarProcessamentoPedidoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = await _pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(2));
        }


        #endregion
    }
}
