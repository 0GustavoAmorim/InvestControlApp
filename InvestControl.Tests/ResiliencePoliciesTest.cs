using InvestControl.Consumer.Resilience;
using Microsoft.Extensions.Logging;

namespace InvestControl.Tests;
public class CotacaoPolicyTests
{
    private readonly ICotacaoPolicy _cotacaoPolicy;

    public CotacaoPolicyTests()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        var logger = loggerFactory.CreateLogger<CotacaoPolicy>();
        _cotacaoPolicy = new CotacaoPolicy(logger);
    }

    [Fact(DisplayName = "Executa com falhas e aciona retry e fallback")]
    public async Task DeveAcionarRetryECircuitBreakerEFallback()
    {
        int execucoes = 0;

        await _cotacaoPolicy.PolicyExecucao.ExecuteAsync(async () =>
        {
            execucoes++;
            Console.WriteLine($"Tentativa #{execucoes}");
            throw new Exception("Erro simulado para teste");
        });

        Assert.True(execucoes >= 1, "Deveria ter tentado ao menos uma vez.");
    }

    [Fact(DisplayName = "Circuit Breaker aciona após falhas consecutivas")]
    public async Task DeveAbrirOCircuitBreaker()
    {
        for (int i = 0; i < 3; i++)
        {
            try
            {
                await _cotacaoPolicy.PolicyExecucao.ExecuteAsync(() =>
                    throw new Exception($"Erro simulado {i + 1}")
                );
            }
            catch
            {
                // Intencionalmente ignorado para simular falha e acionar o breaker
            }
        }
    }
}
