using InvestControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestControl.WebApi.Controllers
{
    [ApiController]
    [Route("api/ativos")]
    public class AtivosController : ControllerBase
    {
        private readonly IAtivoService _ativoService;
        public AtivosController(IAtivoService ativoService)
        {
            _ativoService = ativoService;
        }

        /// <summary>
        /// Retorna a última cotação registrada para o ativo informado
        ///</summary>
        /// <param name="codigo"> Código do ativo (ex: ITUB3) </param>
        /// <returns>Última cotação</returns>
        [HttpGet("{codigo}/cotacoes/ultima")]
        public async Task<IActionResult> ObterUltimaCotacao(string codigo)
        {
            var cotacao = await _ativoService.ObterUltimaCotacao(codigo);
            if (cotacao == null)
            {
                return NotFound($"Cotação para o ativo '{codigo}' não encontrada.");
            }

            return Ok(cotacao);
        }

    }
}
