CREATE TABLE usuarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    perc_corretagem DECIMAL(5,2) NOT NULL
);

CREATE TABLE ativos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    codigo VARCHAR(10) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL
);

CREATE TABLE operacoes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    ativo_id INT NOT NULL,
    quantidade INT NOT NULL,
    preco_unitario DECIMAL(18,4) NOT NULL,
    tipo_operacao VARCHAR(10) NOT NULL,
    corretagem DECIMAL(10,2) NOT NULL,
    data_hora DATETIME2 NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);

CREATE TABLE cotacao (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ativo_id INT NOT NULL,
    preco_unitario DECIMAL(18,4) NOT NULL,
    data_hora DATETIME2 NOT NULL,
    FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);

CREATE TABLE posicoes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    ativo_id INT NOT NULL,
    quantidade INT NOT NULL,
    preco_medio DECIMAL(18,4) NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);
