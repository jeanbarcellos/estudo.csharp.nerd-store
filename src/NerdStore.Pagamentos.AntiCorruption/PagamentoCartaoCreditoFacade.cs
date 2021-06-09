using System;

using NerdStore.Pagamentos.Business;

namespace NerdStore.Pagamentos.AntiCorruption
{
    public class PagamentoCartaoCreditoFacade : IPagamentoCartaoCreditoFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configurationManager;

        public PagamentoCartaoCreditoFacade(
            IPayPalGateway payPalGateway,
            IConfigurationManager configurationManager
        )
        {
            _payPalGateway = payPalGateway;
            _configurationManager = configurationManager;
        }

        public Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento)
        {
            var apiKey = _configurationManager.GetValue("apiKey");
            var encKey = _configurationManager.GetValue("encKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

            var pagamentoResultado = _payPalGateway.CommitTransaction(cardHashKey, pedido.Id.ToString(), pagamento.Valor);

            // TODO: informacao retornada pelo gateway
            var transacao = new Transacao
            {
                PedidoId = pedido.Id,
                PagamentoId = pagamento.Id,
                Total = pedido.Valor
            };

            if (pagamentoResultado)
            {
                transacao.StatusTransacao = StatusTransacao.Pago;
                return transacao;
            }

            transacao.StatusTransacao = StatusTransacao.Recusado;
            return transacao;
        }

    }
}
