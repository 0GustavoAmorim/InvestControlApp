using Polly;
using Microsoft.Extensions.Logging;

namespace InvestControl.Consumer.Resilience
{
    public class CotacaoPolicy : ICotacaoPolicy
    {
        public IAsyncPolicy PolicyExecucao { get; }

        public CotacaoPolicy(ILogger<CotacaoPolicy> logger)
        {
            var retry = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3,
                    i => TimeSpan.FromSeconds(2),
                    (exception, _, retryCount, _) =>
                    {
                        logger.LogWarning($"[Retry {retryCount}] Erro: {exception.Message}");
                    });

            var circuitBreaker = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    2,
                    TimeSpan.FromSeconds(30),
                    onBreak: (exception, timespan) =>
                        logger.LogWarning($"Circuito aberto: {exception.Message}. Esperando {timespan.TotalSeconds}s"),
                    onReset: () =>
                        logger.LogInformation("Circuito fechado, retomando chamadas."),
                    onHalfOpen: () =>
                        logger.LogInformation("Circuito meio aberto, tentando novamente.")
                );

            var fallback = Policy
                .Handle<Exception>()
                .FallbackAsync(
                    fallbackAction: (_, _) =>
                    {
                        logger.LogError("Fallback acionado. Cotação ignorada.");
                        return Task.CompletedTask;
                    },
                    onFallbackAsync: (ex, _) =>
                    {
                        logger.LogWarning($"Entrando no fallback: {ex.Message}");
                        return Task.CompletedTask;
                    });

            PolicyExecucao = fallback.WrapAsync(circuitBreaker).WrapAsync(retry);
        }
    }
}
