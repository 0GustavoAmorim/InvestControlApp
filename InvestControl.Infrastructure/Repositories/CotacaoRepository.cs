using Dapper;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;
using System.Data;

namespace InvestControl.Infrastructure.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly IDbConnection _conn;

        public CotacaoRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public async Task<bool> ExisteCotacao(string ativo, DateTime dataHora)
        {
            var query = @"
                            SELECT COUNT(*) FROM cotacao c
                            JOIN ativos a
                                ON c.ativo_id = a.id
                            WHERE 
                                a.codigo = @ativo 
                                AND c.data_hora = @dataHora
                        ";

            var count = await _conn.ExecuteScalarAsync<int>(query, new { ativo, dataHora });
            return count > 0;
        }

        public async Task<bool> ProcessarCotacao(Cotacao cotacao)
        {
            var ativoId = await _conn.QueryFirstOrDefaultAsync<int?>(
                "SELECT id FROM ativos where codigo = @Codigo",
                new { cotacao.Codigo });

            if (ativoId == null)
                return false;

            var query = @"
                            INSERT INTO cotacao (ativo_id, preco_unitario, data_hora)
                            VALUES (@AtivoId, @precoUnitario, @dataHora)  
                        ";

            await _conn.ExecuteAsync(query, new
            {
                AtivoId = ativoId,
                cotacao.PrecoUnitario,
                cotacao.DataHora
            });

            return true;  
        }
    }
}
