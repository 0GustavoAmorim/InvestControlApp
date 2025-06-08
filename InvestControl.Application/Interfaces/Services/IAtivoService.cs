using InvestControl.Application.DTOs;
using InvestControl.Domain.Entities;

namespace InvestControl.Domain.Interfaces.Services
{
    public interface IAtivoService
    {
        Task<CotacaoDTO?> ObterUltimaCotacao(string codigo);
    }
}
