namespace InvestControl.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public decimal PercentualCorretagem { get; set; }
    }
}
