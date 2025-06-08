using Confluent.Kafka;
using InvestControl.Consumer.Consumers;

namespace InvestControl.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _services;

        public Worker(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<CotacaoConsumer>();

            await consumer.StartAsync(stoppingToken);
        }
    }
}
