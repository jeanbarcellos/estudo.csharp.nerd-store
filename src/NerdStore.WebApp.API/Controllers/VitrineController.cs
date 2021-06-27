using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Controllers
{
    [Route("api/vitrine")]
    public class VitrineController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;

        public VitrineController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            IHttpContextAccessor httpContextAccessor,
            IProdutoAppService produtoAppService
        ) : base(notifications, mediatorHandler, httpContextAccessor)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return Response(await _produtoAppService.ObterTodos());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            return Response(await _produtoAppService.ObterPorId(id));
        }
    }
}
