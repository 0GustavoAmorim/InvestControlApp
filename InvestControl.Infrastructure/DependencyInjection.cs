using InvestControl.Domain.Interfaces.Repositories;
using InvestControl.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace InvestControl.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Configurações de banco de dados
            services.AddTransient<IDbConnection>(sp =>
            {
                var config = sp.GetService<IConfiguration>();
                var connStr = config.GetConnectionString("DefaultConnection");
                return new SqlConnection(connStr);
            });

            // Registre apenas os repositórios da camada de Infrastructure
            services.AddScoped<IAtivoRepository, AtivoRepository>();
            services.AddScoped<IOperacaoRepository, OperacaoRepository>();
            services.AddScoped<IPosicoesRepository, PosicaoRepository>();
            services.AddScoped<ICotacaoRepository, CotacaoRepository>();

            return services;
        }
    }
}
