using InvestControl.Application.Interfaces.Services;
using InvestControl.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestControl.WebApi.Controllers
{
    [ApiController]
    [Route("api/posicoes")]
    public class PosicoesController : ControllerBase
    {
        private readonly IPosicoesService _posicoesService;


        public PosicoesController(IPosicoesService posicoesService)
        {
            _posicoesService = posicoesService;
        }

        [HttpGet("usuarios/{usuarioId}/global")]
        public async Task<IActionResult> ObterPosicaoGlobal(int usuarioId)
        {
            var resultado = await _posicoesService.ObterPosicaoGlobal(usuarioId);
            if (resultado == null)
            {
                return NotFound($"Nenhuma posição global encontrada para o usuário com ID '{usuarioId}'.");
            }
            return Ok(resultado);
        }

        [HttpGet("usuarios/{usuarioId}/por-ativo")]
        public async Task<IActionResult> ObterPosicaoPorPapelInvestidor(int usuarioId)
        {
            var resultado = await _posicoesService.ObterPosicaoPorPapelInvestidor(usuarioId);
            if (resultado == null || !resultado.Any())
            {
                return NotFound($"Nenhuma posição encontrada para o usuário com ID '{usuarioId}'.");
            }
            return Ok(resultado);
        }

        [HttpGet("rankings/maiores-posicoes")]
        public async Task<IActionResult> ObterMaioresPosicoes()
        {
            try
            {
                var maioresPosicoes = await _posicoesService.ObterMaioresPosicoes();
                return Ok(maioresPosicoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter maiores posições: {ex.Message}");
            }
        }
    }
}
