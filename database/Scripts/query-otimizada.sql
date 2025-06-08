SELECT * 
FROM operacoes
WHERE usuario_id = @usuario_id
	AND ativo_id = @ativo_id
	AND data_hora >= DATEADD(DAY, -30, GETDATE())
ORDER BY data_hora DESC;
-- usa o index idx_operacoes_usuario_ativo_data
