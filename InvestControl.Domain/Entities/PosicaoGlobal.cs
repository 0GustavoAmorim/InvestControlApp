namespace InvestControl.Domain.Entities
{
    public class PosicaoGlobal
    {
        public int UsuarioId { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal ValorAtual { get; set; }
        public decimal PnLTotal { get; set; }
    }
}
