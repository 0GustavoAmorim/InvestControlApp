using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InvestControl.Application.DTOs
{
    public class MaioresPosicoesDTO
    {
        [JsonPropertyName("NomeCliente")]
        public string Nome { get; set; }
        [JsonPropertyName("QuantidadeTotal")]
        public int QtdTotal { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
