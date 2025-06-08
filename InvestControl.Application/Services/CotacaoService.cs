using InvestControl.Application.Interfaces.Services;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;

namespace InvestControl.Application.Services
{
    public class CotacaoService : ICotacaoService
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        public CotacaoService(ICotacaoRepository cotacaoRepository)
        {
            _cotacaoRepository = cotacaoRepository;
        }

        public async Task<bool> ExisteCotacao(string ativo, DateTime dataHora)
        {
            return await _cotacaoRepository.ExisteCotacao(ativo, dataHora);
        }
        public async Task<bool> ProcessarCotacao(Cotacao cotacao)
        {
            return await _cotacaoRepository.ProcessarCotacao(cotacao);
        }
    }
}
