using System.Threading.Tasks;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;

namespace NerdStore.Pagamentos.Business
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPagamentoCartaoCreditoFacade _pagamentoCartaoCreditoFacade;

        public PagamentoService(
            IMediatorHandler mediatorHandler,
            IPagamentoRepository pagamentoRepository,
            IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade)
        {
            _mediatorHandler = mediatorHandler;
            _pagamentoRepository = pagamentoRepository;
            _pagamentoCartaoCreditoFacade = pagamentoCartaoCreditoFacade;
        }

        public async Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido)
        {
            var pedido = new Pedido
            {
                Id = pagamentoPedido.PedidoId,
                Valor = pagamentoPedido.Total
            };

            var pagamento = new Pagamento {
                Valor = pagamentoPedido.Total,
                NomeCartao = pagamentoPedido.NomeCartao,
                NumeroCartao = pagamentoPedido.NumeroCartao,
                ExpiracaoCartao = pagamentoPedido.ExpiracaoCartao,
                CVVCartao = pagamentoPedido.CVVCartao,
                PedidoId= pagamentoPedido.PedidoId
            };

            var transacao = _pagamentoCartaoCreditoFacade.RealizarPagamento(pedido, pagamento);

            if (transacao.StatusTransacao == StatusTransacao.Pago)
            {
                pagamento.AdicionarEvento(new PagamentoRealizadoEvent(pedido.Id, pagamentoPedido.ClienteId, transacao.PagamentoId, transacao.Id, pedido.Valor));

                _pagamentoRepository.Adicionar(pagamento);
                _pagamentoRepository.AdicionarTransacao(transacao);

                await _pagamentoRepository.UnitOfWork.Commit();

                return transacao;
            }

            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pagamento", "A operadora recusou o pagamento."));

            await _mediatorHandler.PublicarEvento(new PagamentoRecusadoEvent(pedido.Id, pagamentoPedido.ClienteId, transacao.PagamentoId, transacao.Id, pedido.Valor));

            return transacao;
        }
    }
}
