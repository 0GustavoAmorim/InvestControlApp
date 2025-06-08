using InvestControl.Domain.Entities;

namespace InvestControl.Application.Interfaces.Services
{
    public interface ICotacaoService
    {
        Task<bool> ExisteCotacao(string ativo, DateTime dataHora);
        Task<bool> ProcessarCotacao(Cotacao cotacao);
    }
}
