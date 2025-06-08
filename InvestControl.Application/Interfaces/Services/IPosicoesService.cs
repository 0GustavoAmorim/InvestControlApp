using InvestControl.Application.DTOs;

namespace InvestControl.Application.Interfaces.Services
{
    public interface IPosicoesService
    {
        Task<IEnumerable<PosicaoPorPapelDTO>> ObterPosicaoPorPapelInvestidor(int usuarioId);
        Task<PosicaoGlobalDTO?> ObterPosicaoGlobal(int usuarioId);
        Task<IEnumerable<MaioresPosicoesDTO>> ObterMaioresPosicoes();
    }
}
