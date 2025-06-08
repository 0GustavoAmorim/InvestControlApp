using Dapper;
using InvestControl.Domain.Entities;
using InvestControl.Domain.Interfaces.Repositories;
using System.Data;

namespace InvestControl.Infrastructure.Repositories
{
    public class PosicaoRepository : IPosicoesRepository
    {
        private readonly IDbConnection _conn;

        public PosicaoRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public async Task<PosicaoGlobal?> ObterPosicaoGlobalInvestidor(int usuarioId)
        {
            var query = @"
                            SELECT
                                p.usuario_id AS UsuarioId,
                                SUM(p.quantidade * p.preco_medio) AS TotalInvestido,  
                                SUM(p.quantidade * p.preco_atual) AS ValorAtual,   
                                SUM(p.pnl) AS PnLTotal                                
                            FROM vw_posicoes_com_pnl p
                            WHERE p.usuario_id = @usuarioId
                                GROUP BY p.usuario_id
                        ";

            return await _conn.QueryFirstOrDefaultAsync<PosicaoGlobal>(query, new { usuarioId });
        }

        public async Task<IEnumerable<PosicaoComPnL>> ObterPosicaoPorPapelInvestidor(int usuarioId)
        {
            var query = @"
                            SELECT 
                                v.usuario_id AS UsuarioId,
                                v.codigo_ativo AS CodigoAtivo,
                                v.nome_ativo AS NomeAtivo,
                                v.quantidade AS Quantidade,
                                v.preco_medio AS PrecoMedio,
                                v.preco_atual AS PrecoAtual,
                                v.pnl AS PnL
                            FROM 
                                vw_posicoes_com_pnl v
                            WHERE 
                                v.usuario_id = @usuarioId;
                        ";

            return await _conn.QueryAsync<PosicaoComPnL>(query, new { usuarioId });
        }

        public async Task<IEnumerable<(string Nome, int QtdTotal, decimal ValorTotal)>> ObterMaioresPosicoes()
        {
            var query = @"
                            SELECT TOP 10 
                                u.nome AS NomeCliente,
                                SUM(p.quantidade * c.preco_unitario) AS ValorTotal,
                                SUM(p.quantidade) AS QuantidadeTotal
                            FROM posicoes p
                            JOIN usuarios u ON u.id = p.usuario_id
                            JOIN ativos a ON a.id = p.ativo_id
                            JOIN (
                                SELECT ativo_id, MAX(data_hora) AS ultima_data
                                FROM cotacao
                                GROUP BY ativo_id
                            ) ult ON ult.ativo_id = p.ativo_id
                            JOIN cotacao c ON c.ativo_id = ult.ativo_id AND c.data_hora = ult.ultima_data
                            GROUP BY u.nome
                            ORDER BY ValorTotal DESC;

                        ";

            return await _conn.QueryAsync<(string Nome, int QtdTotal, decimal ValorTotal)>(query);
        }

        public async Task AtualizarPosicaoAsync(int usuarioId, int ativoId, int quantidade, decimal precoUnitario, string tipoOperacao)
        {
            var query = "EXEC atualizar_posicao @usuario_id, @ativo_id, @quantidade, @preco_unitario, @tipo_operacao";

            var parameters = new
            {
                usuario_id = usuarioId,
                ativo_id = ativoId,
                quantidade,
                preco_unitario = precoUnitario,
                tipo_operacao = tipoOperacao
            };

            await _conn.ExecuteAsync(query, parameters);
        }

    }
}
