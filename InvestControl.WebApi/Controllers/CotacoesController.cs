using InvestControl.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestControl.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotacoesController : ControllerBase
    {
        private readonly ICotacaoService _cotacaoService;
        public CotacoesController(ICotacaoService cotacaoService)
        {
            _cotacaoService = cotacaoService;
        }

        [HttpGet("testar-conn")]
        public async Task<IActionResult> TestarConexao()
        {
            var existe = await _cotacaoService.ExisteCotacao("ITUB3", DateTime.Now);
            return Ok(new { sucesso = true, resultado = existe });
        }
    }

}
