using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        #region Atributos de Classe

        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;

        #endregion

        #region Atributos de Instância

        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        // Para não expor a lista, a fim de não ser manipulada externamente
        private readonly List<PedidoItem> _pedidoItens; // Lista Interna
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens; // Lista Externa

        // EF Relation
        public virtual Voucher Voucher { get; private set; }

        #endregion

        #region Construtores

        // EF Contruct
        protected Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }

        public Pedido(Guid clienteId, bool voucherUtilizado, decimal desconto, decimal valorTotal)
        {
            ClienteId = clienteId;
            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
            ValorTotal = valorTotal;
            _pedidoItens = new List<PedidoItem>();
        }

        #endregion

        #region Métodos de comportamentos

        public bool PedidoItemExistente(PedidoItem pedidoItem)
        {
            return _pedidoItens.Any(p => p.ProdutoId == pedidoItem.ProdutoId);
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if (!pedidoItem.EhValido()) return;
            ValidarQuantidadeItemPermitida(pedidoItem);

            pedidoItem.AssociarPedido(Id);

            if (PedidoItemExistente(pedidoItem))
            {
                var itemExistente = _pedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);

                pedidoItem = itemExistente;

                _pedidoItens.Remove(itemExistente);
            }

            pedidoItem.CalcularValor();

            _pedidoItens.Add(pedidoItem);

            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            if (!pedidoItem.EhValido()) return;
            ValidarPedidoItemInexistente(pedidoItem);

            _pedidoItens.Remove(pedidoItem);

            CalcularValorPedido();
        }

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            if (!pedidoItem.EhValido()) return;
            ValidarQuantidadeItemPermitida(pedidoItem);
            ValidarPedidoItemInexistente(pedidoItem);

            pedidoItem.AssociarPedido(Id);

            var itemExistente = _pedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

            _pedidoItens.Remove(itemExistente);
            _pedidoItens.Add(pedidoItem);

            CalcularValorPedido();
        }

        public void AtualizarUnidades(PedidoItem pedidoItem, int unidades)
        {
            pedidoItem.AtualizarUnidades(unidades);

            AtualizarItem(pedidoItem);
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var validationResult = voucher.ValidarSeAplicavel();
            if (!validationResult.IsValid) return validationResult;

            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorPedido();

            return validationResult;
        }

        public void CalcularValorPedido()
        {
            // ValorTotal = PedidoItens.Sum(p => p.CalcularValor());
            ValorTotal = _pedidoItens.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        private void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            var desconto = 0M;
            var valor = ValorTotal;

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        #endregion

        #region Métodos de alteração de estado

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public void IniciarPedido()
        {
            PedidoStatus = PedidoStatus.Iniciado;
        }

        public void FinalizarPedido()
        {
            PedidoStatus = PedidoStatus.Pago;
        }

        public void CancelarPedido()
        {
            PedidoStatus = PedidoStatus.Cancelado;
        }

        #endregion

        #region Métodos privados

        private void ValidarPedidoItemInexistente(PedidoItem pedidoItem)
        {
            if (!PedidoItemExistente(pedidoItem)) throw new DomainException("O item não pertence ao pedido");
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem pedidoItem)
        {
            var quantidadeItems = pedidoItem.Quantidade;
            if (PedidoItemExistente(pedidoItem))
            {
                var itemExistente = _pedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                quantidadeItems += itemExistente.Quantidade;
            }

            if (quantidadeItems > MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} unidades por produto.");
        }

        #endregion

        #region Factory

        // Classe aninhada
        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId
                };

                pedido.TornarRascunho();

                return pedido;
            }
        }

        #endregion
    }
}
