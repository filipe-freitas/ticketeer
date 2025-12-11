# Plano de Produto e Arquitetura: Ticketeer API

## Visão do Produto
Uma API robusta e escalável para gerenciar uma rede de cinemas, permitindo a gestão de catálogo, agendamento de sessões e venda de ingressos com alta concorrência. O projeto serve como um "Laboratório de Tecnologia" para aplicar conceitos "Must Know" do ecossistema .NET (focando em **.NET 10** e versões mais recentes).

## Requisitos Funcionais (O Que Fazer)

### 1. Gestão de Catálogo (Filmes e Cinemas)
*   **Cadastro**: Manter filmes, cinemas e salas.
*   **Busca Avançada**: Permitir busca textual performática por filmes (título, gênero, diretor).
*   **Detalhes**: Exibir ficha técnica completa.

### 2. Gestão de Sessões
*   **Agendamento**: Criar sessões para um filme em uma sala específica.
*   **Validação**: Impedir conflitos de horário na mesma sala.

### 3. Venda de Ingressos (Core Business)
*   **Mapa de Assentos**: Visualizar assentos disponíveis/ocupados em tempo real.
*   **Reserva/Compra**: Bloquear assentos durante a seleção e confirmar a compra.
*   **Concorrência**: Garantir que dois usuários não comprem o mesmo assento simultaneamente.

---

## Requisitos Técnicos (Como Fazer - "Must Know" Mapping)

Aqui mapeamos cada funcionalidade aos itens obrigatórios do roadmap.

### Arquitetura & Design
*   **Arquitetura**: **Clean Architecture** (Domain, Application, Infrastructure, API) para garantir desacoplamento.
*   **Princípios**: **SOLID** aplicado rigorosamente.
*   **API Style**: **Minimal APIs** (para endpoints de alta performance) e **Controllers** (para organização padrão).
*   **Segurança**: **JWT** (JSON Web Tokens) para autenticação stateless e **ASP.NET Core Identity** para gestão de usuários.
*   **Validação**: **FluentValidation** para validação declarativa de inputs.
*   **Resiliência**: **Polly** para retry policies e circuit breakers (ElasticSearch, Redis).
*   **Orquestração Local**: **.NET Aspire** para subir e conectar todos os serviços (API, Banco, Cache) com dashboard de telemetria integrado.
*   **Documentação**: **OpenAPI (Swagger)** para contrato da API.

### Persistência & Dados
*   **Banco Relacional**: **PostgreSQL** (Engine).
*   **ORM (Escrita)**: **EF Core** para gerenciamento de estado e regras de negócio complexas.
*   **Micro-ORM (Leitura)**: **Dapper** para consultas de alta performance e relatórios (CQRS Simplificado).
*   **Cache**: **Redis** para cachear dados de leitura frequente (Detalhes do Filme, Lista de Cinemas).
*   **Busca Textual**: **ElasticSearch** para a busca de filmes (substituindo `LIKE` de banco).

### Fluxos de Trabalho & Mensageria
*   **Mensageria**: **MassTransit** + **RabbitMQ** para comunicação assíncrona entre componentes.
    *   Sincronização do ElasticSearch (quando um filme é criado/atualizado no Postgres).
    *   Processamento pós-compra (emissão de ingresso, envio de e-mail simulado).
*   **Agendamento**: **BackgroundService** + **PeriodicTimer** para tarefas recorrentes (ex: liberar reservas expiradas).

### Observabilidade & Qualidade
*   **Logs**: **Serilog** (structured logging) + **Elasticsearch** (armazenamento) + **Kibana** (visualização).
*   **Métricas & Traces**: **OpenTelemetry** + **Prometheus** (armazenamento) + **Grafana** (dashboards).
*   **Testes de Unidade**: **xUnit** + **NSubstitute** + **Shouldly**.
*   **Testes de Integração**: **WebApplicationFactory** + **TestContainers** para subir banco e Redis reais nos testes.
*   **Testes E2E**: **Playwright** para validar fluxos completos via browser.
*   **Testes de Carga**: **K6** para simular "pico de estreia" na compra de ingressos.
*   **Test Data**: **Bogus** (dados realistas) + **AutoFixture** (geração automática).

### Infraestrutura & DevOps
*   **Versionamento**: **Git** e **GitHub** como repositório central.
*   **Containerização**: **Docker** (gerenciado via Aspire ou Compose).
*   **CI/CD**: **GitHub Actions** para rodar testes e build.
*   **IaC**: **Terraform** (opcional, mas listado) para provisionar recursos se fosse nuvem.

---

## Roadmap de Desenvolvimento Incremental (TDD)

> **Nota**: Cada iteração segue o ciclo: **Escrever Teste → Implementar Feature → Testar → Refatorar**
> 
> Tickets detalhados disponíveis em [tickets.md](tickets.md)

---

### Iteração 0: Fundação & DevOps
**Entregável**: Estrutura de projetos + CI/CD básico + Branch Protection

#### Setup Inicial
1. Criar solution e projetos (Domain, Application, Infrastructure, API, Tests)
2. Configurar referências entre projetos (Clean Architecture)

#### CI/CD
3. Criar `.github/workflows/ci.yml`:
   - `dotnet build`
   - `dotnet test` (ainda sem testes, mas pipeline pronto)
4. **Validação**: Push para GitHub → CI passa (build verde)
5. Configurar branch protection rules (require PR + status check)

---

### Iteração 1: Domain Layer (TDD)
**Entregável**: Entidades de domínio testadas sem dependência de infraestrutura

#### Setup de Testes
1. Adicionar `xUnit`, `NSubstitute`, `Shouldly`, `Bogus` ao projeto Tests
2. Criar `.editorconfig` para padronização de código

#### Domain Entities
3. **Teste**: Criar `MovieTests.cs` (testa criação de Movie com dados válidos)
4. **Implementação**: Criar entidade `Movie` (Id, Title, Description, Duration, ReleaseDate, Genre)
5. **Teste**: Criar `CinemaTests.cs` (testa criação de Cinema)
6. **Implementação**: Criar entidade `Cinema` (Id, Name, Address)
7. **Teste**: Adicionar `HallTests.cs` (testa cálculo de assentos totais)
8. **Implementação**: Criar `Hall` (Id, CinemaId, Name, TotalSeats, RowCount, SeatsPerRow)

#### Value Objects
9. **Teste**: Criar `SeatValueObjectTests.cs` (testa igualdade de Value Object)
10. **Implementação**: Criar Value Object `Seat` (Row, Number)

#### Core Business Logic
11. **Teste**: Criar `SessionTests.cs` (testa `CanPurchaseSeat()` com assento ocupado)
12. **Implementação**: Criar `Session`, `Ticket` e lógica de disponibilidade
13. **Validação**: Rodar `dotnet test` → Todos os testes passam

---

### Iteração 2: Infrastructure + Persistência
**Entregável**: Banco de dados funcional com testes reais

#### .NET Aspire Setup
1. Criar projeto `Ticketeer.AppHost`
2. Configurar PostgreSQL container via Aspire
3. **Validação**: Aspire dashboard acessível em `http://localhost:15888`

#### EF Core Setup
4. Criar `TicketeerDbContext` com DbSet para todas as entidades
5. Configurar Fluent API (FKs, índices, constraints)
6. Criar Migration `InitialCreate`
7. **Validação**: Schema aplicado corretamente no PostgreSQL

#### TestContainers
8. Adicionar `Testcontainers.PostgreSql` ao projeto Tests
9. Criar `CustomWebApplicationFactory` (ADR 006)

#### Testes de Integração
10. **Teste**: Criar `MovieRepositoryTests.cs` (usa TestContainers + Postgres real)
11. **Implementação**: Implementar `MovieRepository` com EF Core
12. **Teste**: Salvar Movie no banco → Recuperar → Validar dados
13. **Validação**: Rodar testes de integração → Postgres sobe/desce automaticamente

---

### Iteração 3: API Básica + Autenticação
**Entregável**: Endpoints funcionais com JWT

#### ASP.NET Core Identity + JWT
1. **Teste**: Criar `AuthenticationTests.cs` (POST /auth/register → retorna 201)
2. **Implementação**:
   - Adicionar Identity ao Infrastructure
   - Criar `ApplicationUser : IdentityUser`
   - Implementar Minimal API `POST /auth/register`
3. **Teste**: `POST /auth/login` com credenciais válidas → retorna JWT
4. **Implementação**: Implementar `POST /auth/login` (gera JWT)
5. **Teste**: Chamar endpoint protegido sem token → 401 Unauthorized
6. **Implementação**: Configurar JWT Bearer Authentication

#### Swagger com Auth
7. Configurar Swagger para aceitar Bearer Token
8. **Teste Manual**: Abrir Swagger → Fazer login → Usar token em endpoint protegido

#### CRUD de Movies
9. **Teste**: Criar `MovieServiceTests.cs` (mock de repository)
10. **Implementação**: Criar `MovieService` (CRUD usando IMovieRepository)
11. **Teste**: Validar `CreateMovieRequest` com título vazio → erro de validação
12. **Implementação**: Adicionar FluentValidation (`CreateMovieRequestValidator`)
13. **Teste**: `POST /movies` com dados válidos → 201 Created
14. **Implementação**: Criar `MoviesController` (CRUD administrativo)

#### Dapper (Read Model)
15. **Teste**: `GET /movies` retorna lista otimizada (sem tracking do EF)
16. **Implementação**: Criar `IQueryService` + query Dapper para listagem

---

### Iteração 4: Core Business - Venda de Ingressos
**Entregável**: Compra de ingressos com proteção de concorrência

#### Sessions CRUD
1. **Teste**: Criar `SessionServiceTests.cs`
2. **Implementação**: Criar `SessionService` e endpoints

#### Ticket Purchase Logic
3. **Teste**: Criar `TicketServiceTests.cs` (compra com assento disponível → sucesso)
4. **Implementação**: Criar `TicketService.PurchaseAsync()`
5. **Teste**: Compra com assento ocupado → erro
6. **Implementação**: Adicionar validação de disponibilidade
7. **Teste**: Duas compras simultâneas do mesmo assento → uma falha (Concurrency Token)
8. **Implementação**: Adicionar `[ConcurrencyCheck]` em `Ticket.SeatNumber`

#### Minimal API (Hot Path)
9. **Teste**: `POST /tickets` com alta concorrência (100 requests simultâneos)
10. **Implementação**: Criar Minimal API `POST /tickets`

#### Testes de Carga
11. **Teste de Carga (K6)**: Simular 1000 usuários → validar sem double-booking

---

### Iteração 5: Cache & Performance
**Entregável**: Redis para endpoints de leitura pesados

#### Redis Setup
1. Configurar Redis via Aspire
2. **Validação**: Redis acessível pelo API

#### Output Caching
3. **Teste**: `GET /movies` sem cache → latência X ms
4. **Implementação**: Configurar Output Caching com Redis
5. **Teste**: `GET /movies` com cache → latência < X/10 ms
6. **Teste**: Criar filme → cache invalida → GET retorna novo filme

#### Cache Manual (Seat Map)
7. **Teste**: `GET /sessions/{id}/seats` usa Redis Hash
8. **Implementação**: Cachear mapa de assentos no Redis
9. **Teste**: Comprar ingresso → cache atualiza → GET retorna assento ocupado

---

### Iteração 6: Busca & Mensageria
**Entregável**: ElasticSearch + MassTransit/RabbitMQ

#### ElasticSearch Setup
1. Configurar Elastic + RabbitMQ via Aspire
2. Criar índice `movies` no Elasticsearch
3. **Teste**: `GET /movies/search?q=avatar` → retorna filmes relevantes
4. **Teste**: Busca com typo "avatr" → retorna "Avatar" (fuzzy matching)

#### MassTransit (Sync Assíncrona)
5. **Teste**: Criar filme → evento `MovieChangedEvent` publicado
6. **Implementação**: Publicar evento ao criar/atualizar Movie
7. **Teste**: Consumer indexa filme no Elastic automaticamente
8. **Implementação**: Criar `MovieChangedConsumer`

#### Polly (Resiliência)
9. **Teste**: Elastic offline → API não quebra (Circuit Breaker)
10. **Implementação**: Adicionar Polly com retry + circuit breaker

---

### Iteração 7: Observabilidade
**Entregável**: Logs estruturados + métricas em tempo real

#### Serilog
1. **Implementação**: Configurar Serilog + Elasticsearch sink
2. **Teste Manual**: Fazer requisição → ver log estruturado no Kibana

#### OpenTelemetry
3. **Implementação**: Configurar OpenTelemetry + Prometheus + Grafana
4. **Teste Manual**: Comprar ingresso → ver métrica "tickets_sold" no Grafana
5. **Implementação**: Criar dashboard com request rate, error rate, P95 latency

---

### Iteração 8: Background Jobs
**Entregável**: Limpeza automática de reservas expiradas

#### BackgroundService
1. **Teste**: Criar reserva expirada → BackgroundService libera assento
2. **Implementação**: Criar `ExpiredReservationCleanupService` com PeriodicTimer
3. **Teste**: Rodar por 10 minutos → validar que reservas antigas foram limpas

---

### Iteração 9: Real-Time (Futuro)
**Entregável**: Mapa de assentos atualizado em tempo real

1. **Teste E2E (Playwright)**: Abrir 2 navegadores → comprar assento no 1º → ver atualização no 2º
2. **Implementação**: Adicionar SignalR + `SeatHub`
3. **Teste**: Comprar ingresso → todos os clientes conectados recebem notificação

---

## Considerações Futuras

Tecnologias que podem ser adicionadas conforme o projeto evolui:

*   **MediatR**: Para implementar CQRS completo e desacoplar Controllers de Handlers.
*   **Nuke**: Para automação de builds complexos quando o pipeline de CI/CD crescer.
