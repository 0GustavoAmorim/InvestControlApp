using InvestControl.Domain.Interfaces.Repositories;
using InvestControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestControl.WebApi.Controllers
{
    [ApiController]
    [Route("api/operacoes")]
    public class OperacoesController : ControllerBase
    {
        private readonly IOperacoesService _operacoesService;
        public OperacoesController(IOperacoesService operacoesService)
        {
            _operacoesService = operacoesService;
        }
        /// <summary>
        /// Retorna total investido por usuário em cada ativo
        ///</summary>
        /// <param name="codigo"> Código do ativo (ex: ITUB3) </param>
        /// <returns>Última cotação</returns>

        [HttpGet("usuarios/{usuarioId}/total-investido")]
        public async Task<IActionResult> ObterTotalInvestidoPorUsuario(int usuarioId)
        {
            var resultado = await _operacoesService.ObterTotalInvestidoInvestidor(usuarioId);

            if (resultado == null || !resultado.Any())
            {
                return NotFound($"Nenhum investimento encontrado para o usuário com ID '{usuarioId}'.");
            }

            return Ok(resultado);
        }

        [HttpGet("usuarios/{usuarioId}/total-corretagens")]
        public async Task<IActionResult> ObterTotalCorretagemPorUsuario(int usuarioId)
        {
            var resultado = await _operacoesService.ObterTotalCorretagemInvestidor(usuarioId);
            if (resultado == null)
                return NotFound("Usuário não encontrado ou sem operações.");

            return Ok(resultado);
        }

        [HttpGet("ativos/{codigo}/preco-medio")]
        public async Task<IActionResult> ObterPrecoMedioAquisicoes(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("Código do ativo é obrigatório.");


            var resultado = await _operacoesService.CalcularPrecoMedio(codigo);

            if (resultado == null || resultado.PrecoMedioAquisicao == 0)
                return NotFound("Não há compras registradas para esse ativo.");

            return Ok(resultado);
        }

        [HttpGet("usuarios/{usuarioId}/precos-medios")]
        public async Task<IActionResult> ObterPrecoMedioAquisicoesUsuario(int usuarioId)
        {
            if (usuarioId == 0 || usuarioId == null)
                return BadRequest("ID do usuário é obrigatório.");

            var resultado = await _operacoesService.CalcularPrecoMedioUsuario(usuarioId);

            if (resultado == null || !resultado.Any())
                return NotFound("Não há compras registradas para esse usuário.");

            return Ok(resultado);
        }

        [HttpGet("rankings/maiores-corretagens")]
        public async Task<IActionResult> ObterMaioresCorretagens()
        {
            try
            {
                var maioresCorretagens = await _operacoesService.ObterMaioresCorretagens();
                return Ok(maioresCorretagens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter maiores corretagens: {ex.Message}");
            }
        }

        [HttpGet("corretagens/total")]
        public async Task<IActionResult> ObterValorTotalCorretagens()
        {
            try
            {
                var valorTotal = await _operacoesService.ObterValorTotalCorretagens();
                return Ok(valorTotal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter valor total de corretagens: {ex.Message}");
            }
        }
    }
}
