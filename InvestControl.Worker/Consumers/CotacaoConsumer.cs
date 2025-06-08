using Confluent.Kafka;
using InvestControl.Consumer.Handlers;

namespace InvestControl.Consumer.Consumers
{
    public class CotacaoConsumer
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CotacaoConsumer> _logger;
        private readonly CotacaoHandler _cotacaoHandler;

        public CotacaoConsumer(IConfiguration config, ILogger<CotacaoConsumer> logger, CotacaoHandler cotacaoHandler)
        {
            _config = config;
            _logger = logger;
            _cotacaoHandler = cotacaoHandler;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:Broker"],
                GroupId = _config["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(_config["Kafka:Topic"]);

            _logger.LogInformation("Kafka consumer iniciado.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);

                        if (!string.IsNullOrWhiteSpace(result.Message?.Value))
                        {
                            await _cotacaoHandler.HandleAsync(result.Message.Value);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Erro ao consumir mensagem do Kafka: {ex.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Erro inesperado: {ex.Message}");
                    }
                }
            }
            finally
            {
                consumer.Close();
                _logger.LogInformation("Kafka consumer fechado com sucesso.");
            }

        }
    }
}
