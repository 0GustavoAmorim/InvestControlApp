using InvestControl.Application.DTOs;

namespace InvestControl.Domain.Interfaces.Services
{
    public interface IOperacoesService
    {
        Task<IEnumerable<TotalInvestidoPorAtivoDTO>> ObterTotalInvestidoInvestidor(int usuarioId);
        Task<TotalCorretagemDTO> ObterTotalCorretagemInvestidor(int usuarioId);
        Task<PrecoMedioDTO?> CalcularPrecoMedio(string codigo);
        Task<IEnumerable<PrecoMedioUsuarioDTO?>> CalcularPrecoMedioUsuario(int usuarioId);
        Task<IEnumerable<MaioresCorretagensDTO>> ObterMaioresCorretagens();
        Task<decimal> ObterValorTotalCorretagens();
    }
}
