using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using NerdStore.WebApp.API.Models;

namespace NerdStore.WebApp.API.Controllers
{
    [Authorize]
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IPedidoQueries _pedidoQueries;
        private readonly IMediatorHandler _mediatorHandler;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public CarrinhoController(
            INotificationHandler<DomainNotification> notifications,
            IProdutoAppService produtoAppService,
            IMediatorHandler mediatorHandler,
            IPedidoQueries pedidoQueries,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings
        ) : base(notifications, mediatorHandler, httpContextAccessor)
        {
            _produtoAppService = produtoAppService;
            _mediatorHandler = mediatorHandler;
            _pedidoQueries = pedidoQueries;
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("api/carrinho")]
        public async Task<IActionResult> Get()
        {
            return Response(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("api/carrinho")]
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
        [Route("api/carrinho/{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ItemViewModel item)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            var command = new AtualizarItemPedidoCommand(ClienteId, produto.Id, item.Quantidade);
            await _mediatorHandler.EnviarComando(command);

            return Response();
        }

        [HttpDelete]
        [Route("api/carrinho/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediatorHandler.EnviarComando(command);

            return Response();
        }
    }
}