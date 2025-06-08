using InvestControl.Consumer.Consumers;
using InvestControl.Consumer.Handlers;
using InvestControl.Consumer;
using InvestControl.Application;
using InvestControl.Infrastructure;
using Polly;
using InvestControl.Consumer.Resilience;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();


builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<CotacaoHandler>();
builder.Services.AddScoped<CotacaoConsumer>();
builder.Services.AddSingleton<ICotacaoPolicy, CotacaoPolicy>();

var host = builder.Build();
host.Run();
