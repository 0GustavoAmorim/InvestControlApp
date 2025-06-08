using InvestControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestControl.Domain.Interfaces.Repositories
{
    public interface IPosicoesRepository
    {
        Task<IEnumerable<PosicaoComPnL>> ObterPosicaoPorPapelInvestidor(int usuarioId);
        Task<PosicaoGlobal?> ObterPosicaoGlobalInvestidor(int usuarioId);
        Task<IEnumerable<(string Nome, int QtdTotal, decimal ValorTotal)>> ObterMaioresPosicoes();
    }
}
