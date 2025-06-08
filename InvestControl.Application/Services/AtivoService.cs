using InvestControl.Application.DTOs;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;
using InvestControl.Domain.Interfaces.Services;

namespace InvestControl.Application.Services
{
    public class AtivoService : IAtivoService
    {
        private readonly IAtivoRepository _ativoRepository;
        public AtivoService(IAtivoRepository ativoRepository)
        {
            _ativoRepository = ativoRepository;
        }

        public async Task<CotacaoDTO?> ObterUltimaCotacao(string codigo)
        {
            var cotacao = await _ativoRepository.ObterUltimaCotacao(codigo);

            if (cotacao == null)
                return null;

            return new CotacaoDTO
            {
                Codigo = codigo,
                PrecoAtual = cotacao.PrecoUnitario,
                DataHora = cotacao.DataHora
            };
        }
    }
}
