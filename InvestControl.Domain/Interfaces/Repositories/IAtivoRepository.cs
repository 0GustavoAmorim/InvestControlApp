using InvestControl.Domain.Entities;

namespace InvestControl.Domain.Interfaces.Repositories
{
    public interface IAtivoRepository
    {
        Task<Cotacao?> ObterUltimaCotacao(string codigo);
    }
}
