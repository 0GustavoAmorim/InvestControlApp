using InvestControl.Domain.Entities;

namespace InvestControl.Domain.Interfaces.Repositories
{
    public interface ICotacaoRepository
    {
        Task<bool> ExisteCotacao(string ativo, DateTime dataHora);
        Task<bool> ProcessarCotacao(Cotacao cotacao);
    }
}
