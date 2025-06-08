using Dapper;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;
using System.Data;

namespace InvestControl.Infrastructure.Repositories
{
    public class AtivoRepository : IAtivoRepository
    {
        private readonly IDbConnection _conn;

        public AtivoRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        public Task<Cotacao?> ObterUltimaCotacao(string codigo)
        {
            var query = @"
                           SELECT TOP 1 
                                c.preco_unitario AS PrecoUnitario, 
                                c.data_hora AS DataHora, 
                                c.ativo_id AS AtivoId, 
                                a.codigo AS Codigo
                            FROM cotacao c
                            INNER JOIN ativos a ON c.ativo_id = a.id
                            WHERE a.codigo = @codigo 
                            ORDER BY c.data_hora DESC

                        ";

            return _conn.QueryFirstOrDefaultAsync<Cotacao>(query, new { codigo });

        }
    }
}
