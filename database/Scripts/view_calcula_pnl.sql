CREATE OR ALTER VIEW vw_posicoes_com_pnl AS
SELECT 
    p.usuario_id,
    p.ativo_id,
    a.codigo AS codigo_ativo,
    a.nome AS nome_ativo,
    p.quantidade,
    p.preco_medio,
    c.preco_unitario AS preco_atual,
    ROUND((c.preco_unitario - p.preco_medio) * p.quantidade, 2) AS pnl
FROM 
    posicoes p
JOIN 
    ativos a ON a.id = p.ativo_id
JOIN 
    (
        SELECT c1.ativo_id, c1.preco_unitario
        FROM cotacao c1
        INNER JOIN (
            SELECT ativo_id, MAX(data_hora) AS max_data
            FROM cotacao
            GROUP BY ativo_id
        ) c2 ON c1.ativo_id = c2.ativo_id AND c1.data_hora = c2.max_data
    ) c ON c.ativo_id = p.ativo_id;
