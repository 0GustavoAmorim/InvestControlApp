using InvestControl.Application.DTOs;
using InvestControl.Application.Interfaces.Services;
using InvestControl.Consumer.Resilience;
using InvestControl.Domain.Entities;
using Polly;
using System.Diagnostics;
using System.Text.Json;

namespace InvestControl.Consumer.Handlers
{
    public class CotacaoHandler
    {
        private readonly ICotacaoService _cotacaoService;
        private readonly ILogger<CotacaoHandler> _logger;
        private readonly IAsyncPolicy _policy;

        public CotacaoHandler(ICotacaoService cotacaoService, ILogger<CotacaoHandler> logger, ICotacaoPolicy policy)
        {
            _cotacaoService = cotacaoService;
            _logger = logger;
            _policy = policy.PolicyExecucao;
        }

        public async Task HandleAsync(string message)
        {
            var correlationId = Guid.NewGuid().ToString();
            using var logScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            });

            _logger.LogInformation("Iniciando processamento de mensagem. CorrelationId: {CorrelationId}", correlationId);
            var stopwatch = Stopwatch.StartNew();

            CotacaoKafkaDTO? kafkaDto;

            try
            {
                kafkaDto = JsonSerializer.Deserialize<CotacaoKafkaDTO>(message);
                if (kafkaDto == null)
                {
                    _logger.LogWarning("Mensagem inválida: null ou malformatada. CorrelationId: {CorrelationId}", correlationId);
                    return;
                }

                _logger.LogDebug("Mensagem deserializada com sucesso. Ticker: {Ticker}, Preço: {Preco}, Data: {DataHora}, CorrelationId: {CorrelationId}",
                    kafkaDto.Ticker, kafkaDto.Preco, kafkaDto.DataHora, correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deserializar mensagem. CorrelationId: {CorrelationId}, Erro: {ErrorMessage}",
                    correlationId, ex.Message);
                return;
            }

            var cotacao = new Cotacao
            {
                Codigo = kafkaDto.Ticker,
                PrecoUnitario = kafkaDto.Preco,
                DataHora = kafkaDto.DataHora
            };

            try
            {
                var existe = await _cotacaoService.ExisteCotacao(cotacao.Codigo, cotacao.DataHora);
                if (existe)
                {
                    _logger.LogInformation("Cotação já atualizada, ignorando. Ticker: {Ticker}, Data: {DataHora}, CorrelationId: {CorrelationId}",
                        cotacao.Codigo, cotacao.DataHora, correlationId);
                    return;
                }
                else
                {
                    _logger.LogInformation("Cotação não encontrada, prosseguindo com o processamento. Ticker: {Ticker}, Data: {DataHora}, CorrelationId: {CorrelationId}",
                        cotacao.Codigo, cotacao.DataHora, correlationId);
                }

                await _policy.ExecuteAsync(async () =>
                {
                    _logger.LogDebug("Executando operação com política de resiliência. Ticker: {Ticker}, CorrelationId: {CorrelationId}",
                        cotacao.Codigo, correlationId);

                    var salva = await _cotacaoService.ProcessarCotacao(cotacao);
                    if (salva)
                    {
                        _logger.LogInformation("Cotação atualizada com sucesso. Ticker: {Ticker}, Preço: {Preco}, CorrelationId: {CorrelationId}",
                            cotacao.Codigo, cotacao.PrecoUnitario, correlationId);
                    }
                    else
                    {
                        _logger.LogWarning("Falha ao processar cotação. Ticker: {Ticker}, CorrelationId: {CorrelationId}",
                            cotacao.Codigo, correlationId);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado ao processar cotação. Ticker: {Ticker}, CorrelationId: {CorrelationId}, Erro: {ErrorMessage}",
                    cotacao.Codigo, correlationId, ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("Processamento concluído em {ElapsedMs}ms. Ticker: {Ticker}, CorrelationId: {CorrelationId}",
                    stopwatch.ElapsedMilliseconds, cotacao.Codigo, correlationId);
            }
        }
    }
}
