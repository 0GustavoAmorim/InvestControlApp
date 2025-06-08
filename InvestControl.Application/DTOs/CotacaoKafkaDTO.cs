namespace InvestControl.Application.DTOs
{
    public class CotacaoKafkaDTO
    {
        public string Ticker { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataHora { get; set; }
    }
}
