
-- Inserts de Exemplo

-- Usuários
INSERT INTO usuarios (nome, email, perc_corretagem) VALUES 
('Alice Marques', 'alice@email.com', 0.25),
('Bruno Lima', 'bruno@email.com', 0.15);

-- Ativos
INSERT INTO ativos (codigo, nome) VALUES 
('PETR4', 'Petrobras PN'),
('VALE3', 'Vale ON');

-- Operações
INSERT INTO operacoes (usuario_id, ativo_id, quantidade, preco_unitario, tipo_operacao, corretagem, data_hora) VALUES 
(1, 1, 100, 27.35, 'COMPRA', 5.50, GETDATE()),
(1, 2, 50, 68.10, 'COMPRA', 4.75, GETDATE()),
(2, 1, 200, 26.80, 'VENDA', 8.20, GETDATE());

-- Cotações
INSERT INTO cotacao (ativo_id, preco_unitario, data_hora) VALUES 
(1, 28.10, GETDATE()),
(2, 69.25, GETDATE());

-- Posições
INSERT INTO posicoes (usuario_id, ativo_id, quantidade, preco_medio) VALUES
(1, 1, 15, 32.00),
(1, 2, 8, 122.50),
(2, 3, 12, 170.00);
