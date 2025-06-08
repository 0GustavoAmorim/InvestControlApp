using InvestControl.Application.DTOs;
using InvestControl.Application.Interfaces.Services;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;

namespace InvestControl.Application.Services
{
    public class PosicoesService : IPosicoesService
    {
        private readonly IPosicoesRepository _posicoesRepository;

        public PosicoesService(IPosicoesRepository posicoesRepository)
        {
            _posicoesRepository = posicoesRepository;
        }

        public async Task<PosicaoGlobalDTO?> ObterPosicaoGlobal(int usuarioId)
        {
            var posicao = await _posicoesRepository.ObterPosicaoGlobalInvestidor(usuarioId);
            if (posicao == null)
                return null;

            return new PosicaoGlobalDTO
            {
                UsuarioId = posicao.UsuarioId,
                TotalInvestido = posicao.TotalInvestido,
                ValorAtual = posicao.ValorAtual,
                PnLTotal = posicao.PnLTotal
            };
        }

        public async Task<IEnumerable<PosicaoPorPapelDTO>> ObterPosicaoPorPapelInvestidor(int usuarioId)
        {
            var posicoes = await _posicoesRepository.ObterPosicaoPorPapelInvestidor(usuarioId);

            return posicoes.Select(p => new PosicaoPorPapelDTO
            {
                UsuarioId = p.UsuarioId,
                CodigoAtivo = p.CodigoAtivo,
                NomeAtivo = p.NomeAtivo,
                Quantidade = p.Quantidade,
                PrecoMedio = p.PrecoMedio,
                PrecoAtual = p.PrecoAtual,
                PnL = p.PnL
            });
        }
        public async Task<IEnumerable<MaioresPosicoesDTO>> ObterMaioresPosicoes()
        {
            var posicoes = await _posicoesRepository.ObterMaioresPosicoes();

            return posicoes.Select(p => new MaioresPosicoesDTO
            {
                Nome = p.Nome,
                QtdTotal = p.QtdTotal,
                ValorTotal = p.ValorTotal
            });
        }
    }
}
