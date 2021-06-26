using System;
using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NerdStore.Vendas.Application.Queries;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using Microsoft.AspNetCore.Http;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoQueries _pedidoQueries;

        public PedidoController(
            IPedidoQueries pedidoQueries,
            IMediatorHandler mediatorHandler,
            INotificationHandler<DomainNotification> notifications,
            IHttpContextAccessor httpContextAccessor
        )
            : base(notifications, mediatorHandler, httpContextAccessor)
        {
            _pedidoQueries = pedidoQueries;
        }

        [Route("meus-pedidos")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidoQueries.ObterPedidosCliente(ClienteId));
        }
    }
}
