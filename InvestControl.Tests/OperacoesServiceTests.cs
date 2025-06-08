using InvestControl.Application.Services;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;
using InvestControl.Domain.Interfaces.Services;
using NSubstitute;

namespace InvestControl.Tests
{
    public class OperacoesServiceTests
    {
        private readonly IOperacaoRepository _repoSub;
        private readonly IOperacoesService _service;

        public OperacoesServiceTests()
        {
            _repoSub = Substitute.For<IOperacaoRepository>();
            _service = new OperacoesService(_repoSub);
        }

        [Fact]
        public async Task CalcularPrecoMedioAquisicao_DeveRetornarPrecoCorreto()
        {
            // Arrange
            var codigoAtivo = "ITUB3";
            var operacoes = new List<Operacao>
            {
                new Operacao { Quantidade = 10, PrecoUnitario = 15 },
                new Operacao { Quantidade = 20, PrecoUnitario = 18 }
            };

            _repoSub.ObterOperacoesPorAtivo(codigoAtivo)
                .Returns(operacoes);

            var resultado = await _service.CalcularPrecoMedio(codigoAtivo);
            Assert.Equal(17.00m, resultado.PrecoMedioAquisicao);
        }

        [Fact]
        public async Task CalcularPrecoMedio_QuandoNaoHouverOperacoes_DeveRetornarNull()
        {
            var codigoAtivo = "ITUB3";

            _repoSub.ObterOperacoesPorAtivo(codigoAtivo).Returns(new List<Operacao>());

            var resultado = await _service.CalcularPrecoMedio(codigoAtivo);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task CalcularPrecoMedio_QuandoPrecoTotalForZero_DeveRetornarZero()
        {
            // Arrange
            var codigoAtivo = "ITUB3";

            var operacoes = new List<Operacao>
            {
                new Operacao { Quantidade = 10, PrecoUnitario = 0 },
                new Operacao { Quantidade = 5, PrecoUnitario = 0 }
            };

            _repoSub.ObterOperacoesPorAtivo(codigoAtivo).Returns(operacoes);

            var resultado = await _service.CalcularPrecoMedio(codigoAtivo);
            Assert.Equal(0, resultado.PrecoMedioAquisicao);
        }

        [Fact]
        public async Task CalcularPrecoMedio_ValoresQuebrados_DeveSerPreciso()
        {
            // Arrange
            var codigoAtivo = "ITUB3";
            var operacoes = new List<Operacao>
            {
                new Operacao { Quantidade = 3, PrecoUnitario = 10.33m },
                new Operacao { Quantidade = 7, PrecoUnitario = 20.77m }
            };

            _repoSub.ObterOperacoesPorAtivo(codigoAtivo)
                .Returns(operacoes);

            var resultado = await _service.CalcularPrecoMedio(codigoAtivo);
            var esperado = ((3 * 10.33m) + (7 * 20.77m)) / 10;

            Assert.Equal(esperado, resultado.PrecoMedioAquisicao);
        }

    }
}
