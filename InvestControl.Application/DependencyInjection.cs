using InvestControl.Application.Interfaces.Services;
using InvestControl.Application.Services;
using InvestControl.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvestControl.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registre apenas os serviços da camada de Application
            services.AddScoped<IAtivoService, AtivoService>();
            services.AddScoped<IOperacoesService, OperacoesService>();
            services.AddScoped<IPosicoesService, PosicoesService>();
            services.AddScoped<ICotacaoService, CotacaoService>();

            return services;
        }
    }
}
