using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar Voucher Tipo Valor Válido")]
        [Trait("Unit", "Vendas.Domain - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            // Arrange
            var codigo = "PROMO-15-REAIS";
            var valorDesconto = 15;
            var quantidade = 1;
            var tipoDescontoVoucher = TipoDescontoVoucher.Valor;
            var dataValidade = DateTime.Now.AddDays(15);
            var ativo = true;
            var utilizado = false;

            var voucher = new Voucher(codigo, null, valorDesconto, quantidade,
                tipoDescontoVoucher, dataValidade, ativo, utilizado);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(codigo, voucher.Codigo);
            Assert.Null(voucher.Percentual);
            Assert.Equal(valorDesconto, voucher.ValorDesconto);
            Assert.Equal(quantidade, voucher.Quantidade);
            Assert.Equal(tipoDescontoVoucher, voucher.TipoDescontoVoucher);
            Assert.Equal(dataValidade, voucher.DataValidade);
            Assert.True(voucher.Ativo);
            Assert.False(voucher.Utilizado);
        }

        [Fact(DisplayName = "Validar Voucher Tipo Valor Inválido")]
        [Trait("Unit", "Vendas.Domain - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherAplicavelValidator.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.ValorDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Validar Voucher Porcentagem Válido")]
        [Trait("Unit", "Vendas.Domain - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarValido()
        {
            var voucher = new Voucher("PROMO-15-OFF", 15, null, 1,
                TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Porcentagem Inválido")]
        [Trait("Unit", "Vendas.Domain - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarInvalido()
        {
            var voucher = new Voucher("", null, null, 0,
                TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherAplicavelValidator.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidator.PercentualErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
