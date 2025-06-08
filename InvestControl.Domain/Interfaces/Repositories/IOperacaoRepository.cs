using InvestControl.Domain.Entities;
using InvestControl.Domain.ValueObjects;

namespace InvestControl.Domain.Interfaces.Repositories
{
    public interface IOperacaoRepository
    {
        Task<(int UsuarioId, decimal TotalCorretagem)> ObterTotalCorretagemPorInvestidor(int usuarioId);
        Task<IEnumerable<TotalInvestidoPorAtivo>> ObterTotalInvestidoInvestidor(int usuarioId);
        Task<IEnumerable<Operacao>> ObterOperacoesPorAtivo(string codigo);
        Task<IEnumerable<Operacao>> ObterOperacoesPorUsuario(int usuarioId);
        Task<IEnumerable<(string Nome, decimal TotalCorretagem)>> ObterMaioresCorretagens();
        Task<decimal> ObterValorTotalCorretagens();
    }
}
