using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using NerdStore.WebApp.API.Models;

namespace NerdStore.WebApp.API.Controllers
{
    [Authorize]
    [Route("api/carrinho")]
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IPedidoQueries _pedidoQueries;
        private readonly IMediatorHandler _mediatorHandler;

        public CarrinhoController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            IHttpContextAccessor httpContextAccessor,
            IProdutoAppService produtoAppService,
            IPedidoQueries pedidoQueries
        ) : base(notifications, mediatorHandler, httpContextAccessor)
        {
            _produtoAppService = produtoAppService;
            _mediatorHandler = mediatorHandler;
            _pedidoQueries = pedidoQueries;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            return Response(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post([FromBody] ItemViewModel item)
        {
            var produto = await _produtoAppService.ObterPorId(item.Id);
            if (produto == null) return BadRequest();

            if (produto.QuantidadeEstoque < item.Quantidade)
            {
                NotificarErro("ErroValidacao", "Produto com estoque insuficiente");
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, item.Quantidade, produto.Valor);
            await _mediatorHandler.EnviarComando(command);

            return Response();
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ItemViewModel item)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            var command = new AtualizarItemPedidoCommand(ClienteId, produto.Id, item.Quantidade);
            await _mediatorHandler.EnviarComando(command);

            return Response();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediatorHandler.EnviarComando(command);

            return Response();
        }

        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher([FromBody] VoucherViewModel voucher)
        {
            var command = new AplicarVoucherPedidoCommand(ClienteId, voucher.Codigo);
            await _mediatorHandler.EnviarComando(command);

            return Response();
        }

        [HttpPost]
        [Route("iniciar-pedido")]
        public async Task<IActionResult> IniciarPedido([FromBody] CarrinhoPagamentoViewModel carrinhoPagamentoViewModel)
        {
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);
            if (carrinho == null) return BadRequest();

            var command = new IniciarPedidoCommand(
                carrinho.PedidoId, ClienteId, carrinho.ValorTotal,
                carrinhoPagamentoViewModel.NomeCartao, carrinhoPagamentoViewModel.NumeroCartao,
                carrinhoPagamentoViewModel.ExpiracaoCartao, carrinhoPagamentoViewModel.CVVCartao
            );

            await _mediatorHandler.EnviarComando(command);

            return Response();
        }
    }
}