using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace NerdStore.Vendas.Domain
{
    public class Voucher : Entity
    {
        public string Codigo { get; private set; }
        public decimal? Percentual { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataUtilizacao { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        // EF Rel.
        public virtual ICollection<Pedido> Pedidos { get; private set; }

        // EF Contruct
        protected Voucher()
        { }

        public Voucher(string codigo, decimal? percentual, decimal? valorDesconto, int quantidade,
            TipoDescontoVoucher tipoDescontoVoucher, DateTime dataValidade, bool ativo, bool utilizado)
        {
            Codigo = codigo;
            Percentual = percentual;
            ValorDesconto = valorDesconto;
            Quantidade = quantidade;
            TipoDescontoVoucher = tipoDescontoVoucher;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
        }

        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidator().Validate(this);
        }
    }
}
