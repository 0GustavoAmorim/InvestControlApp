using InvestControl.Application.DTOs;
using InvestControl.Domain.Interfaces.Repositories;
using InvestControl.Domain.Interfaces.Services;

namespace InvestControl.Application.Services
{
    public class OperacoesService : IOperacoesService
    {
        private readonly IOperacaoRepository _operacaoRepository;

        public OperacoesService(IOperacaoRepository operacaoRepository)
        {
            _operacaoRepository = operacaoRepository;
        }


        public async Task<TotalCorretagemDTO> ObterTotalCorretagemInvestidor(int usuarioId)
        {
            var resultado = await _operacaoRepository.ObterTotalCorretagemPorInvestidor(usuarioId);
            if (resultado == default)
                return null;

            return new TotalCorretagemDTO
            {
                UsuarioId = resultado.UsuarioId,
                TotalCorretagem = resultado.TotalCorretagem
            };
        }

        public async Task<IEnumerable<TotalInvestidoPorAtivoDTO>> ObterTotalInvestidoInvestidor(int usuarioId)
        {
            var totais = await _operacaoRepository.ObterTotalInvestidoInvestidor(usuarioId);
            if (totais == null || !totais.Any())
            {
                return Enumerable.Empty<TotalInvestidoPorAtivoDTO>();
            }

            return totais.Select(t => new TotalInvestidoPorAtivoDTO
            {
                UsuarioId = t.UsuarioId,
                CodigoAtivo = t.CodigoAtivo,
                TotalInvestido = t.TotalInvestido
            }).ToList();
        }

        public async Task<PrecoMedioDTO?> CalcularPrecoMedio(string codigo)
        {
            var operacoes = await _operacaoRepository.ObterOperacoesPorAtivo(codigo);

            if (operacoes == null || !operacoes.Any())
            {
                return null;
            }

            var totalInvestido = operacoes.Sum(o => o.Quantidade * o.PrecoUnitario);
            var totalQtd = operacoes.Sum(o => o.Quantidade);

            var precoMedio = totalQtd == 0 ? 0 : totalInvestido / totalQtd;

            return new PrecoMedioDTO
            {
                CodigoAtivo = codigo,
                PrecoMedioAquisicao = precoMedio
            };
        }

        public async Task<IEnumerable<PrecoMedioUsuarioDTO?>> CalcularPrecoMedioUsuario(int usuarioId)
        {
            var operacoes = await _operacaoRepository.ObterOperacoesPorUsuario(usuarioId);

            if (operacoes == null || !operacoes.Any())
            {
                return Enumerable.Empty<PrecoMedioUsuarioDTO>();
            }

            var precosMedios = operacoes
                .GroupBy(o => o.CodigoAtivo)
                .Select(g =>
                {
                    var totalInvestido = g.Sum(o => o.Quantidade * o.PrecoUnitario);
                    var totalQtd = g.Sum(o => o.Quantidade);
                    var precoMedio = totalQtd == 0 ? 0 : totalInvestido / totalQtd;

                    return new PrecoMedioUsuarioDTO
                    {
                        CodigoAtivo = g.Key,
                        Quantidade = totalQtd,
                        PrecoMedioAquisicao = precoMedio
                    };
                });

            return precosMedios;
        }

        public async Task<IEnumerable<MaioresCorretagensDTO>> ObterMaioresCorretagens()
        {
            var corretagens = await _operacaoRepository.ObterMaioresCorretagens();

            return corretagens.Select(c => new MaioresCorretagensDTO
            {
                Nome = c.Nome,
                TotalCorretagem = c.TotalCorretagem
            });
        }

        public async Task<decimal> ObterValorTotalCorretagens()
        {
            var total = await _operacaoRepository.ObterValorTotalCorretagens();
            return total;
        }

    }
}
