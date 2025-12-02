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

---

### Iteração 0: Fundação
**Entregável**: Estrutura de projetos + CI/CD básico

#### Setup Inicial
1. Criar solution e projetos (Domain, Application, Infrastructure, API, Tests)
2. Configurar referências entre projetos (Clean Architecture)
3. Configurar .NET Aspire (AppHost + PostgreSQL container)
4. Configurar Git/GitHub + `.gitignore`

#### CI/CD Básico
5. Criar `.github/workflows/ci.yml`:
   - `dotnet build`
   - `dotnet test` (ainda sem testes, mas pipeline pronto)
6. **Teste**: Push para GitHub → CI passa (build verde)

---

### Iteração 1: Domain + Testes de Unidade
**Entregável**: Entidades de domínio testadas

#### Domain Entities
1. **Teste**: Criar `MovieTests.cs` (testa criação de Movie com dados válidos)
2. **Implementação**: Criar entidade `Movie` (Id, Title, Description, Duration, ReleaseDate, Genre)
3. **Teste**: Adicionar `HallTests.cs` (testa cálculo de assentos totais)
4. **Implementação**: Criar `Hall` (Id, CinemaId, Name, TotalSeats, RowCount, SeatsPerRow)
5. **Teste**: Criar `SessionTests.cs` (testa `CanPurchaseSeat()` com assento ocupado)
6. **Implementação**: Criar `Session`, `Ticket` e lógica de disponibilidade
7. **Teste**: Criar `SeatValueObjectTests.cs` (testa igualdade de Value Object)
8. **Implementação**: Criar Value Object `Seat` (Row, Number)

#### Configurar Testes
9. Adicionar `xUnit`, `NSubstitute`, `Shouldly`, `Bogus` ao projeto Tests
10. **Teste**: Rodar `dotnet test` → Todos os testes passam

---

### Iteração 2: Infrastructure + Testes de Integração
**Entregável**: Banco de dados funcional com testes reais

#### EF Core Setup
1. **Teste**: Criar `MovieRepositoryTests.cs` (usa TestContainers + Postgres real)
2. **Implementação**: 
   - Criar `TicketeerDbContext` com DbSet<Movie>
   - Implementar `MovieRepository` com EF Core
   - Criar Migration `InitialCreate`
3. **Teste**: Salvar Movie no banco → Recuperar → Validar dados
4. **Implementação**: Adicionar DbSets restantes (Cinema, Hall, Session, Ticket)
5. **Teste**: Criar `SessionRepositoryTests.cs` (testa relacionamentos Movie → Session → Hall)
6. **Implementação**: Configurar Fluent API (FKs, índices, constraints)

#### TestContainers Setup
7. Adicionar `Testcontainers.PostgreSql` ao projeto Tests
8. Criar `CustomWebApplicationFactory` (ADR 006)
9. **Teste**: Rodar testes de integração → Postgres sobe/desce automaticamente

---

### Iteração 3: Autenticação + Testes E2E Básicos
**Entregável**: Login/Register funcionando com testes

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

---

### Iteração 4: CRUD de Filmes + Validação
**Entregável**: Gerenciamento de filmes com validação

#### Application Layer (Movies)
1. **Teste**: Criar `MovieServiceTests.cs` (mock de repository)
2. **Implementação**: Criar `MovieService` (CRUD usando IMovieRepository)
3. **Teste**: Validar `CreateMovieRequest` com título vazio → erro de validação
4. **Implementação**: Adicionar FluentValidation (`CreateMovieRequestValidator`)

#### API (Movies Controller)
5. **Teste**: `POST /movies` com dados válidos → 201 Created
6. **Implementação**: Criar `MoviesController` (CRUD administrativo)
7. **Teste**: `GET /movies/{id}` → retorna filme correto
8. **Teste**: `PUT /movies/{id}` → atualiza filme
9. **Teste**: `DELETE /movies/{id}` → remove filme

#### Dapper (Read Model)
10. **Teste**: `GET /movies` retorna lista otimizada (sem tracking do EF)
11. **Implementação**: Criar `IQueryService` + query Dapper para listagem

---

### Iteração 5: Compra de Ingressos + Concorrência
**Entregável**: Venda de ingressos com proteção contra double-booking

#### Ticket Purchase Logic
1. **Teste**: Criar `TicketServiceTests.cs` (compra com assento disponível → sucesso)
2. **Implementação**: Criar `TicketService.PurchaseAsync()`
3. **Teste**: Compra com assento ocupado → erro
4. **Implementação**: Adicionar validação de disponibilidade
5. **Teste**: Duas compras simultâneas do mesmo assento → uma falha (Concurrency Token)
6. **Implementação**: Adicionar `[ConcurrencyCheck]` em `Ticket.SeatNumber`

#### Minimal API (Hot Path)
7. **Teste**: `POST /tickets` com alta concorrência (100 requests simultâneos)
8. **Implementação**: Criar Minimal API `POST /tickets`
9. **Teste de Carga (K6)**: Simular 1000 usuários → validar sem double-booking

---

### Iteração 6: Redis Cache
**Entregável**: Cache funcionando em endpoints de leitura

#### Output Caching
1. **Teste**: `GET /movies` sem cache → latência X ms
2. **Implementação**: Configurar Redis via Aspire + Output Caching
3. **Teste**: `GET /movies` com cache → latência < X/10 ms
4. **Teste**: Criar filme → cache invalida → GET retorna novo filme

#### Cache Manual (Seat Map)
5. **Teste**: `GET /sessions/{id}/seats` usa Redis Hash
6. **Implementação**: Cachear mapa de assentos no Redis
7. **Teste**: Comprar ingresso → cache atualiza → GET retorna assento ocupado

---

### Iteração 7: ElasticSearch + Mensageria
**Entregável**: Busca textual funcionando com sincronização assíncrona

#### ElasticSearch Setup
1. **Teste**: `GET /movies/search?q=avatar` → retorna filmes relevantes
2. **Implementação**: Configurar Elastic via Aspire + criar índice `movies`
3. **Teste**: Busca com typo "avatr" → retorna "Avatar" (fuzzy matching)

#### MassTransit (Sync Assíncrona)
4. **Teste**: Criar filme → evento `MovieChangedEvent` publicado
5. **Implementação**: Publicar evento ao criar/atualizar Movie
6. **Teste**: Consumer indexa filme no Elastic automaticamente
7. **Implementação**: Criar `MovieChangedConsumer`

#### Polly (Resiliência)
8. **Teste**: Elastic offline → API não quebra (Circuit Breaker)
9. **Implementação**: Adicionar Polly com retry + circuit breaker

---

### Iteração 8: Observabilidade
**Entregável**: Logs estruturados + métricas em tempo real

#### Serilog
1. **Implementação**: Configurar Serilog + Elasticsearch sink
2. **Teste Manual**: Fazer requisição → ver log estruturado no Kibana

#### OpenTelemetry
3. **Implementação**: Configurar OpenTelemetry + Prometheus + Grafana
4. **Teste Manual**: Comprar ingresso → ver métrica "tickets_sold" no Grafana
5. **Implementação**: Criar dashboard com request rate, error rate, P95 latency

---

### Iteração 9: Background Jobs
**Entregável**: Limpeza automática de reservas expiradas

#### BackgroundService
1. **Teste**: Criar reserva expirada → BackgroundService libera assento
2. **Implementação**: Criar `ExpiredReservationCleanupService` com PeriodicTimer
3. **Teste**: Rodar por 10 minutos → validar que reservas antigas foram limpas

---

### Iteração 10 (Futuro): Real-Time com SignalR
**Entregável**: Mapa de assentos atualizado em tempo real

1. **Teste E2E (Playwright)**: Abrir 2 navegadores → comprar assento no 1º → ver atualização no 2º
2. **Implementação**: Adicionar SignalR + `SeatHub`
3. **Teste**: Comprar ingresso → todos os clientes conectados recebem notificação

---

## Considerações Futuras

Tecnologias que podem ser adicionadas conforme o projeto evolui:

*   **MediatR**: Para implementar CQRS completo e desacoplar Controllers de Handlers.
*   **Nuke**: Para automação de builds complexos quando o pipeline de CI/CD crescer.
