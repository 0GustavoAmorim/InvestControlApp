namespace InvestControl.Domain.ValueObjects
{
    public class TotalInvestidoPorAtivo
    {
        public int UsuarioId { get; set; }
        public string CodigoAtivo { get; set; }
        public decimal TotalInvestido { get; set; }
    }
}
