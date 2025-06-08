namespace InvestControl.Application.DTOs
{
    public class PosicaoGlobalDTO
    {
        public int UsuarioId { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal ValorAtual { get; set; }
        public decimal PnLTotal { get; set; }
    }
}
