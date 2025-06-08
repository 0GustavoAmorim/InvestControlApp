using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestControl.Domain.Entities
{
    public class PosicaoComPnL
    {
        public int UsuarioId { get; set; }
        public string CodigoAtivo { get; set; } = string.Empty;
        public string NomeAtivo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoMedio { get; set; }
        public decimal PrecoAtual { get; set; } // <- vem da view
        public decimal PnL { get; set; }
    }
}
