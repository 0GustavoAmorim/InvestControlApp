using Dapper;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;
using InvestControl.Domain.ValueObjects;
using System.Data;

namespace InvestControl.Infrastructure.Repositories
{
    public class OperacaoRepository : IOperacaoRepository
    {
        private readonly IDbConnection _conn;
        public OperacaoRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public Task<(int UsuarioId, decimal TotalCorretagem)> ObterTotalCorretagemPorInvestidor(int usuarioId)
        {
            var query = @"
                            SELECT usuario_id, SUM(corretagem) AS TotalCorretagem
                            FROM operacoes
                            WHERE usuario_id = @usuarioId
                            GROUP BY usuario_id;
                        ";
            return _conn.QuerySingleAsync<(int UsuarioId, decimal TotalCorretagem)>(query, new { usuarioId });
        }

        public async Task<IEnumerable<TotalInvestidoPorAtivo>> ObterTotalInvestidoInvestidor(int usuarioId)
        {
            var query = @"
                            SELECT 
                                o.usuario_id AS UsuarioId,
                                a.codigo AS CodigoAtivo, 
                                SUM(o.quantidade * o.preco_unitario) AS TotalInvestido
                            FROM 
                                operacoes o
                            INNER JOIN 
                                ativos a ON o.ativo_id = a.id
                            WHERE 
                                o.usuario_id = @usuarioId
                                AND o.tipo_operacao = 'Compra'
                            GROUP BY 
                                o.usuario_id,
                                a.codigo
                        ";
            return await _conn.QueryAsync<TotalInvestidoPorAtivo>(query, new { usuarioId });
        }

        public async Task<IEnumerable<Operacao>> ObterOperacoesPorAtivo(string codigo)
        {
            var query = @"
                            SELECT 
                               o.Id,
                               o.usuario_id AS UsuarioId,
                               o.ativo_id AS AtivoId,
                               a.codigo AS CodigoAtivo,  
                               o.quantidade AS Quantidade,
                               o.preco_unitario AS PrecoUnitario,
                               o.tipo_operacao AS TipoOperacao,
                               o.corretagem AS Corretagem,
                               o.data_hora AS DataHora
                            FROM 
                                operacoes o
                            INNER JOIN 
                                ativos a ON o.ativo_id = a.id
                            WHERE 
                                a.codigo = @codigo
                                AND o.tipo_operacao = 'COMPRA'
                        ";
            return await _conn.QueryAsync<Operacao>(query, new { codigo });
        }

        public async Task<IEnumerable<Operacao>> ObterOperacoesPorUsuario(int usuarioId)
        {
            var query = @"
                            SELECT 
                               o.Id,
                               o.usuario_id AS UsuarioId,
                               o.ativo_id AS AtivoId,
                               a.codigo AS CodigoAtivo,  
                               o.quantidade AS Quantidade,
                               o.preco_unitario AS PrecoUnitario,
                               o.tipo_operacao AS TipoOperacao,
                               o.corretagem AS Corretagem,
                               o.data_hora AS DataHora
                            FROM 
                                operacoes o
                            INNER JOIN 
                                ativos a ON o.ativo_id = a.id
                            WHERE 
                                o.usuario_id = @usuarioId
                                AND o.tipo_operacao = 'COMPRA'
                        ";

            return await _conn.QueryAsync<Operacao>(query, new { usuarioId });
        }

        public async Task<IEnumerable<(string Nome, decimal TotalCorretagem)>> ObterMaioresCorretagens()
        {
            var query = @"
                            SELECT TOP 10 u.nome, SUM(o.corretagem) AS TotalCorretagem
                            FROM operacoes o
                            INNER JOIN usuarios u ON u.id = o.usuario_id
                            GROUP BY u.nome
                            ORDER BY TotalCorretagem DESC;
                        ";

            return await _conn.QueryAsync<(string Nome, decimal ValorTotal)>(query);
        }

        public async Task<decimal> ObterValorTotalCorretagens()
        {
            var query = @"
                            SELECT SUM(corretagem)
                            FROM operacoes
                        ";
            return await _conn.ExecuteScalarAsync<decimal>(query);
        }
    }
}
