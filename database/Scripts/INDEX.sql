CREATE NONCLUSTERED INDEX idx_op_usuario_ativo_data
ON operacoes (usuario_id, ativo_id, data_hora)

CREATE NONCLUSTERED INDEX idx_posicao_usuario_ativo
ON posicoes (usuario_id, ativo_id);

CREATE NONCLUSTERED INDEX idx_cotacao_ativo_data
ON cotacao (ativo_id, data_hora);
