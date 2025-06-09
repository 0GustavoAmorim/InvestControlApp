# InvestControl - Sistema de Controle de Investimentos

## Descrição

Projeto desenvolvido para atender a um teste técnico com foco em arquitetura limpa, mensageria com Kafka e resiliência. A aplicação permite o controle de investimentos de clientes, incluindo posições, cotações, operações e rankings.

## Tecnologias Utilizadas

- **.NET 8**
- **ASP.NET Web API**
- **Kafka (Confluent Kafka via Docker Compose)**
- **Dapper**
- **SQL Server**
- **Swagger (OpenAPI 3.0)**
- **Worker Service .NET**
- **Tailwind CSS**
- **JS**
- **HTML**

## Repositórios
- InvestControlApp: `InvestControlApp` ([Repositório Principal](https://github.com/0GustavoAmorim/InvestControlApp)])
- Producer Kafka: `InvestControl.Producer` ([Repositório Producer](https://github.com/0GustavoAmorim/InvestControl.Producer))
- Frontend: `InvestControl.UI` ([Repositório Frontend](https://github.com/0GustavoAmorim/InvestControl.UI))

## Arquitetura

- Separação em camadas: `Domain`, `Application`, `Infrastructure`, `Consumer`, `WebApi`, `Tests`
- Utilização de princípios DDD
- Repositórios isolados com Dapper
- Mensageria assíncrona com Kafka para ingestão de cotações

## Funcionalidades Implementadas (API)

- GET `/api/ativos/{codigo}/cotacoes/ultima` – Última cotação de ativo
- GET `/api/posicoes/usuarios/{usuarioId}/global` – Posição global consolidada
- GET `/api/posicoes/usuarios/{usuarioId}/por-ativo` – Posição por ativo
- GET `/api/posicoes/rankings/maiores-posicoes` – Top 10 posições por valor
- GET `/api/operacoes/usuarios/{usuarioId}/total-investido` – Total investido por ativo
- GET `/api/operacoes/usuarios/{usuarioId}/total-corretagens` – Total de corretagens
- GET `/api/operacoes/ativos/{codigo}/preco-medio` – Preço médio de ativo
- GET `/api/operacoes/usuarios/{usuarioId}/precos-medios` – Preço médio por usuário
- GET `/api/operacoes/rankings/maiores-corretagens` – Top 10 usuários por corretagem
- GET `/api/operacoes/corretagens/total` – Valor financeiro total em corretagens

![image](https://github.com/user-attachments/assets/79d52438-33f9-4cbe-b660-0b818d5d854b)


## Frontend (`InvestControl.UI`)

O frontend foi desenvolvido com HTML, Tailwind CSS e JavaScript puro, consumindo os endpoints da API para exibir os principais dados de investimento por usuário.

### Funcionalidades exibidas:

- **Posição Global**: mostra total investido, valor atual e lucro/prejuízo
- **Posição por Ativo**: lista os ativos com preço médio, cotação e lucro/prejuízo
- **Total Investido por Ativo**: gráfico de barras com Chart.js
- **Total de Corretagens**: valor acumulado de corretagens pagas

### Organização dos Arquivos:

- `index.html`: estrutura principal
- `script.js`: consumo da API e atualização da interface

### Tecnologias:

- **Tailwind CSS**
- **Chart.js**
- **Axios**
![image](https://github.com/user-attachments/assets/fcce3c4c-aa7b-422e-bb69-b9c384824fce)


## Testes Unitários e Mutantes

Foram criados testes unitários com `xUnit` e NSubstitute para o método de cálculo de preço médio.

### Testes Mutantes - Stryker.NET
Conceito

Alteramos o código propositalmente (mutação) para verificar se os testes detectam erros.

- ✅ **Testes que falham com mutação**: indicam boa cobertura
- 🚫 **Testes que passam com erro**: revelam testes ineficazes

### Exemplo:

```csharp
// Original
var precoMedio = totalQtd == 0 ? 0 : totalInvestido / totalQtd;

// Mutação
var precoMedio = totalQtd == 0 ? 0 : totalInvestido * totalQtd;
```

## Mensageria Kafka

- InvestControl.Producer consome da [BRAPI.dev](https://brapi.dev) e envia para o tópico `cotacoes`
- Worker Service .NET `InvestControl.Consumer` atua como consumer, persistindo cotações no banco
- Não registra cotações duplicadas validando por `preco_atual e data_hora`
- Integração assíncrona

## Engenharia do Caos

Implementada com **Polly**:

- **Retry**: até 3 vezes com delay de 2s
- **Circuit Breaker**: fecha consumo temporariamente após 3 falhas
- **Fallback**: ignora mensagens inválidas
- **Idempotência**: não grava cotações duplicadas `(ativo_id, data_hora)`

## Escalabilidade e Performance

### Auto-scaling Horizontal

- Múltiplos workers podem processar cotações em paralelo
- Kafka distribui automaticamente entre partições e consumidores

### Balanceamento de Carga

- Kafka gerencia consumer groups com round-robin """ENTENDER"""

## Padrão RESTful

- Verbos HTTP adequados (GET)
- Recursos aninhados: `/usuarios/{id}/...`
- Segmentos organizados por contexto (`posicoes`, `operacoes`, `ativos`)


## Modelagem de Dados e Índices

### Índices Criados

```sql
-- Operações por usuário, ativo e data
CREATE NONCLUSTERED INDEX idx_op_usuario_ativo_data
ON operacoes (usuario_id, ativo_id, data_hora);
- Melhor para filtro por usuário e ativo nos últimos 30 dias

-- Posição atual por usuário e ativo
CREATE NONCLUSTERED INDEX idx_posicao_usuario_ativo
ON posicao (usuario_id, ativo_id);
- Usado em cálculos de posição e P&L

-- Última cotação
CREATE NONCLUSTERED INDEX idx_cotacao_ativo_data
ON cotacao (ativo_id, data_hora);
- Otimiza queries com `TOP 1 ORDER BY data_hora DESC`
```

### Tipos de Dados

| Campo                       | Tipo Dado     | Justificativa                                        |
| --------------------------- | ------------- | ---------------------------------------------------- |
| Id                          | INT IDENTITY  | Auto incremento e eficiente para indexação           |
| nome, email, codigo         | VARCHAR(100) e VARCHAR(10)     | Flexível e controlado, evita uso excessivo de espaço |
| preco_unitario, preco_medio | DECIMAL(18,4) | Alta precisão para valores financeiros               |
| corretagem                  | DECIMAL(10,2) | Precisão em centavos                                 |
| quantidade                  | INT           | Suporta grande volume de ativos                      |
| data_hora                   | DATETIME2     | Alta precisão temporal                               |
| tipo_operacao               | VARCHAR(10)   | Simples, mas poderia ser CHAR ou ID com tabela separada para os tipos                        |
| perc_corretagem             | DECIMAL(5,2)  | Representa porcentagem com precisão (ex: 0.25%)      |                           

## Views para P&L

- P&L depende da cotação mais atual
- Armazenar o P&L resultaria em valor defasado rapidamente
- Entendo que não é o melhor caminho armazenar esse campo visto que ele é derivado de um calculo que muda em questão de milisegundos com base na cotação.
- Utiliza índice `idx_cotacao_ativo_data` para performance

### Quando poderia ser armazenado o P&L
- Para auditoria
- Para relatórios com valores do momento da operações

## Cálculo do Preço Médio

- Preço médio representa o custo médio histórico de aquisição
- Baseado apenas em **compras**
- Não depende da cotação atual
- Cotação entendo que afeta apenas P&L
- Representa o custo histórico
- Calculado como `(preço unitário * quantidade) / total quantidade comprada`

## Documentação

- Documentado com Swagger (OpenAPI 3.0)
- Disponível em `/swagger/index.html`
- Selecionar a versão (Manual)

## Como Executar os Projetos

### 🧠 InvestControlApp (Backend API)
```
- Definir como projetos de inicialização multipla InvestControl.WebApi e InvestControl.Consumer
- Crie o banco
- Altere para sua connection string no AppSettings

dotnet run
```


- Script de banco em /database/script_banco.sql
- DDL e dados iniciais (INSERTs)
- Execute no SSMS


### ⚙️ InvestControl.Producer (Kafka Producer)

```bash
cd InvestControl.Producer
docker compose up -d
dotnet run

'token para teste presente'
```

#### docker-compose usado para kafka
```yaml docker compose
version: '3.8'

services:
  zookeeper:
    image: confluentinc/cp-zookeeper:7.5.0
    ports:
      - "2181:2181"
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181

  kafka:
    image: confluentinc/cp-kafka:7.5.0
    ports:
      - "9092:9092"
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:29092,PLAINTEXT_HOST://localhost:9092
      KAFKA_LISTENERS: PLAINTEXT://0.0.0.0:29092,PLAINTEXT_HOST://0.0.0.0:9092
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1

  kafka-ui:
    image: provectuslabs/kafka-ui:latest
    ports:
      - "8080:8080"
    environment:
      KAFKA_CLUSTERS_0_NAME: local-kafka
      KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS: kafka:29092
      KAFKA_CLUSTERS_0_ZOOKEEPER: zookeeper:2181

```

### 💻 InvestControl.UI (Frontend)

```bash
cd InvestControl.UI
# Abrir index.html diretamente no navegador
```
