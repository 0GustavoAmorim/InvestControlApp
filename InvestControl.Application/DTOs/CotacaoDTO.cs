namespace InvestControl.Application.DTOs
{
    public class CotacaoDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public decimal PrecoAtual { get; set; }
        public DateTime DataHora { get; set; }
    }
}
