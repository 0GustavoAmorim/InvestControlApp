using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestControl.Application.DTOs
{
    public class PosicaoPorPapelDTO
    {
        public int UsuarioId { get; set; }
        public string CodigoAtivo { get; set; } = string.Empty;
        public string NomeAtivo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoMedio { get; set; }
        public decimal PrecoAtual { get; set; }
        public decimal PnL { get; set; }
    }
}
