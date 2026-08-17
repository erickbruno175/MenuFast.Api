# 🍔 MenuFast

<p align="center">
  <strong>Sistema de gestão para restaurantes, lanchonetes e estabelecimentos alimentícios.</strong>
</p>

<p align="center">
  <img src="arquitetura-img.png" alt="Arquitetura do MenuFast">
</p>

---

## 📌 Sobre o projeto

O **MenuFast** é uma plataforma de gestão desenvolvida para centralizar as principais operações de restaurantes, lanchonetes e outros estabelecimentos do segmento alimentício.

A aplicação concentra recursos de **cadastro, produtos, mesas, pedidos, operação da cozinha, PDV, financeiro e relatórios**, utilizando uma API como núcleo central da aplicação.

O sistema também foi pensado para trabalhar com múltiplas lojas, permitindo a separação dos dados por estabelecimento.

### Principais funcionalidades

* 🏪 Gestão de lojas
* 👤 Gestão de usuários
* 📂 Categorias e produtos
* 🪑 Mesas
* 🛒 Pedidos
* 👨‍🍳 KDS para operação da cozinha
* 💰 PDV / Caixa
* 💳 Controle financeiro
* 📊 Relatórios e dashboards
* 🔐 Autenticação e autorização
* 🏢 Multi-tenant
* ⚡ Cache e sessão com Redis
* 🔗 Integração com serviços externos
* 📱 Aplicativo para garçons

---

# 🏗️ Arquitetura

A arquitetura do MenuFast é baseada em uma **API REST desenvolvida em .NET 8**, responsável por centralizar as regras de negócio, autenticação, persistência e comunicação com os demais componentes do sistema.

As aplicações clientes se comunicam com a API através de **HTTPS**.

```text
                         ┌──────────────────────┐
                         │       CLIENTES       │
                         │                      │
                         │  Web                 │
                         │  Aplicativo Garçom   │
                         │  PDV / Caixa         │
                         │  KDS - Cozinha       │
                         └──────────┬───────────┘
                                    │
                                  HTTPS
                                    │
                                    ▼
                    ┌──────────────────────────────┐
                    │       MENUFAST API           │
                    │           .NET 8             │
                    │                              │
                    │ Controllers                  │
                    │ Application                  │
                    │ Domain                       │
                    │ Middlewares                  │
                    │ Persistence                  │
                    │ Util / Helpers               │
                    └─────────────┬────────────────┘
                                  │
                     ┌────────────┼────────────┐
                     │            │            │
                     ▼            ▼            ▼
               ┌──────────┐ ┌──────────┐ ┌──────────────┐
               │ SQL      │ │  Redis   │ │  Serviços    │
               │ Server   │ │          │ │  Externos    │
               │          │ │ Cache    │ │              │
               │ Dados    │ │ Sessão   │ │ Pagamentos   │
               │ Tenant   │ │ Locks    │ │ E-mail       │
               └──────────┘ └──────────┘ │ WhatsApp     │
                                         │ Webhooks     │
                                         └──────────────┘
```

A arquitetura completa está representada no diagrama disponível no início deste documento.

---

# 🧱 Organização da API

O projeto **MenuFast.Api** possui sua estrutura organizada por responsabilidades.

```text
MenuFast.Api/
│
├── Api/
│   ├── Application/
│   ├── Controllers/
│   ├── Domain/
│   ├── Middlewares/
│   ├── Persistence/
│   └── Util/
│       └── Helpers/
│
├── Migrations/
│   ├── ... Migrations do Entity Framework Core
│   └── MenuFastContextModelSnapshot.cs
│
├── Properties/
│   └── launchSettings.json
│
├── .editorconfig
├── .gitattributes
├── .gitignore
├── MenuFast.Api.csproj
├── MenuFast.Api.http
└── MenuFast.Api.sln
```

### Api/Application

Contém os serviços e componentes responsáveis pela execução dos casos de uso da aplicação.

Responsabilidades:

* Serviços
* Casos de uso
* Regras da aplicação
* DTOs
* Validações
* Orquestração das operações

---

### Api/Controllers

Responsável pela exposição dos endpoints HTTP da aplicação.

Os controllers recebem as requisições dos clientes, realizam as validações necessárias e encaminham as operações para as camadas responsáveis.

---

### Api/Domain

Concentra os elementos relacionados ao domínio do sistema.

Responsabilidades:

* Entidades
* Regras de domínio
* Modelos
* Enumerações
* Estruturas relacionadas ao negócio

---

### Api/Middlewares

Contém componentes executados durante o processamento das requisições HTTP.

Podem ser utilizados para:

* Autenticação
* Tratamento de exceções
* Logs
* Validações
* Controle de requisições

---

### Api/Persistence

Responsável pela persistência e comunicação com o banco de dados.

Utiliza **Entity Framework Core** para mapeamento e acesso aos dados.

Responsabilidades:

* DbContext
* Configurações das entidades
* Repositórios
* Consultas
* Persistência
* Transações
* Integração com SQL Server

---

### Api/Util/Helpers

Contém classes auxiliares utilizadas por diferentes partes da aplicação.

Esses componentes concentram funcionalidades reutilizáveis que não pertencem diretamente às regras de negócio.

---

# 🗄️ Banco de dados

O MenuFast utiliza **SQL Server** como banco de dados principal.

O acesso ao banco é realizado através do **Entity Framework Core**.

### Recursos utilizados

* Entity Framework Core
* Migrations
* Relacionamentos
* Consultas
* Persistência
* Transações
* Controle de alterações
* Backup e restauração

As alterações estruturais do banco são controladas através das migrations do Entity Framework Core.

```text
Migrations/
│
├── 20260802153652_InicioBancoDeDados.cs
├── 20260802153652_InicioBancoDeDados.Designer.cs
├── ...
└── MenuFastContextModelSnapshot.cs
```

---

# ⚡ Redis

O **Redis** é utilizado como infraestrutura auxiliar da aplicação.

Entre suas possíveis utilizações estão:

* Cache
* Sessões
* Controle de concorrência
* Locks distribuídos
* Dados temporários

O Redis reduz a necessidade de consultas repetitivas ao banco de dados e auxilia em operações que exigem controle de concorrência.

---

# 🔐 Segurança

A API utiliza mecanismos de autenticação e autorização para proteger os recursos do sistema.

### Recursos

* JWT
* Autenticação de usuários
* Autorização
* Controle de permissões
* Policies
* Proteção dos endpoints
* Middleware de autenticação
* Tratamento de exceções
* Controle de sessão

O objetivo é garantir que cada usuário tenha acesso somente aos recursos permitidos dentro do contexto da aplicação.

---

# 🏢 Multi-tenant

O MenuFast possui suporte à arquitetura **multi-tenant**.

A ideia é permitir que diferentes estabelecimentos utilizem a mesma aplicação mantendo seus dados separados logicamente.

```text
                 MENUFAST
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
     Loja A       Loja B      Loja C
        │           │           │
        ▼           ▼           ▼
      Dados       Dados       Dados
      próprios    próprios    próprios
```

O contexto da loja é considerado durante o acesso e manipulação dos dados.

---

# 🛒 Fluxo principal do pedido

O fluxo principal do sistema pode ser representado da seguinte forma:

```text
1. Cliente acessa o cardápio
              │
              ▼
2. Seleciona os produtos
              │
              ▼
3. Pedido enviado para a API
              │
              ▼
4. API registra o pedido
              │
              ▼
5. Pedido disponibilizado no KDS
              │
              ▼
6. Cozinha realiza a produção
              │
              ▼
7. Pedido atualizado
              │
              ▼
8. Garçom acompanha o status
              │
              ▼
9. Pedido finalizado
              │
              ▼
10. Fechamento no PDV
```

---

# 👨‍🍳 KDS — Kitchen Display System

A operação da cozinha utiliza um **KDS (Kitchen Display System)** em vez de impressoras para receber os pedidos.

O KDS apresenta os pedidos diretamente em uma tela de produção, permitindo que a equipe da cozinha acompanhe os itens que precisam ser preparados.

### Fluxo

```text
Pedido
  │
  ▼
API
  │
  ▼
Banco de dados
  │
  ▼
KDS
  │
  ├── Novo
  │
  ├── Em preparação
  │
  ├── Pronto
  │
  └── Finalizado
```

Isso permite que o processo de produção seja acompanhado digitalmente, reduzindo a dependência de impressão física na cozinha.

---

# 📱 Aplicativo do garçom

O aplicativo utilizado pelos garçons permite realizar operações relacionadas ao atendimento.

Entre as operações estão:

* Visualização de mesas
* Abertura de pedidos
* Inclusão de produtos
* Alteração de itens
* Observações
* Acompanhamento do pedido
* Consulta do status da cozinha

A aplicação mobile se comunica com a API através de HTTPS.

---

# 🖥️ Aplicação Web

A aplicação Web é destinada principalmente à administração e operação do estabelecimento.

### Principais áreas

* Cadastros
* Produtos
* Categorias
* Mesas
* Clientes
* Pedidos
* PDV
* Financeiro
* Relatórios
* Configurações

---

# 💻 PDV / Caixa

O módulo de PDV concentra as operações relacionadas ao fechamento e movimentação do caixa.

### Funcionalidades

* Abertura de caixa
* Fechamento de caixa
* Recebimentos
* Pagamentos
* Sangria
* Contas
* Movimentações financeiras

---

# 💰 Financeiro

O módulo financeiro permite acompanhar as movimentações financeiras do estabelecimento.

### Recursos

* Contas a pagar
* Contas a receber
* Fluxo de caixa
* Relatórios financeiros
* DRE

---

# 📊 Relatórios

O sistema centraliza os dados operacionais para geração de relatórios e indicadores.

Exemplos:

* Vendas
* Produtos
* Comissões
* Desempenho
* Movimentação financeira
* Dashboards

---

# 🔗 Integrações

A arquitetura permite comunicação com serviços externos através de APIs, Webhooks e outros mecanismos de integração.

### Integrações previstas

* 💳 Serviços de pagamento
* 📧 E-mail / SMTP
* 📱 WhatsApp / notificações
* 🔗 Webhooks
* ☁️ Armazenamento de imagens
* 🌐 CDN

As integrações externas ficam separadas da lógica principal sempre que possível, facilitando a substituição ou evolução dos provedores utilizados.

---

# 💳 Pagamentos

O módulo de pagamentos pode se comunicar com provedores externos através de APIs.

Os **Webhooks** permitem que o provedor informe automaticamente à API eventos relacionados às transações.

```text
┌───────────────┐
│    MenuFast   │
│      API      │
└───────┬───────┘
        │
        │ API
        ▼
┌───────────────┐
│   Provedor    │
│  Pagamento    │
└───────┬───────┘
        │
        │ Webhook
        ▼
┌───────────────┐
│    MenuFast   │
│      API      │
└───────────────┘
```

---

# 🧩 Módulos de negócio

## Cadastros

* Lojas
* Usuários
* Categorias
* Produtos
* Mesas
* Clientes
* Impressoras / dispositivos, quando aplicável

## Pedidos

* Abertura
* Itens
* Observações
* Status
* Histórico

## PDV / Caixa

* Contas
* Pagamentos
* Recebimentos
* Fechamento
* Sangria

## Financeiro

* Contas a pagar
* Contas a receber
* Fluxo de caixa
* Relatórios
* DRE

## Relatórios

* Vendas
* Produtos
* Comissões
* Desempenho
* Dashboards

## Configurações

* Preferências
* Permissões
* Integrações
* Parâmetros
* Backup

---

# 🛠️ Tecnologias

| Tecnologia            | Utilização               |
| --------------------- | ------------------------ |
| C#                    | Backend                  |
| .NET 8                | Plataforma principal     |
| ASP.NET Core          | API REST                 |
| Entity Framework Core | ORM / Persistência       |
| SQL Server            | Banco de dados           |
| Redis                 | Cache / Sessão / Locks   |
| Angular               | Frontend Web             |
| TypeScript            | Frontend                 |
| PrimeNG               | Componentes de interface |
| Bootstrap             | Layout                   |
| .NET MAUI             | Aplicativo Mobile        |
| SQLite                | Persistência local       |
| JWT                   | Autenticação             |
| FluentValidation      | Validação                |
| AutoMapper            | Mapeamento de objetos    |
| Swagger / OpenAPI     | Documentação da API      |
| Docker                | Containerização          |
| Serilog               | Logging                  |

---

# 🧪 Desenvolvimento

Para executar a API localmente, é necessário ter instalado:

* .NET 8 SDK
* SQL Server
* Redis
* Git

Clone o repositório:

```bash
git clone <URL_DO_REPOSITORIO>
```

Entre no diretório:

```bash
cd MenuFast.Api
```

Restaure as dependências:

```bash
dotnet restore
```

Execute as migrations:

```bash
dotnet ef database update
```

Execute a aplicação:

```bash
dotnet run
```

A documentação da API pode ser acessada através do **Swagger**, quando habilitada no ambiente de desenvolvimento.

---

# 🗂️ Estrutura atual do repositório

```text
MenuFast.Api/
│
├── Api/
│   │
│   ├── Application/
│   │   └── Serviços e casos de uso
│   │
│   ├── Controllers/
│   │   └── Endpoints HTTP
│   │
│   ├── Domain/
│   │   └── Entidades e regras de domínio
│   │
│   ├── Middlewares/
│   │   └── Middlewares da aplicação
│   │
│   ├── Persistence/
│   │   └── Persistência e Entity Framework Core
│   │
│   └── Util/
│       └── Helpers e utilitários
│
├── Migrations/
│   └── Migrations do Entity Framework Core
│
├── Properties/
│   └── launchSettings.json
│
├── .editorconfig
├── .gitattributes
├── .gitignore
├── MenuFast.Api.csproj
├── MenuFast.Api.http
└── MenuFast.Api.sln
```

---

# 📐 Princípios da arquitetura

O projeto busca manter uma separação clara entre:

```text
Controllers
     │
     ▼
Application
     │
     ▼
Domain
     │
     ▼
Persistence
     │
     ▼
SQL Server
```

Além disso, componentes transversais como autenticação, autorização, logging, validação, cache e tratamento de exceções são utilizados para manter responsabilidades comuns centralizadas.

---

# 🎯 Objetivos

A arquitetura do MenuFast foi estruturada buscando:

* Separação de responsabilidades
* Organização do código
* Facilidade de manutenção
* Reutilização de componentes
* Segurança
* Isolamento de dados entre lojas
* Escalabilidade
* Integração com aplicações Web e Mobile
* Comunicação com serviços externos
* Operação digital da cozinha através de KDS

---

# 📄 Documentação da arquitetura

O diagrama completo da arquitetura está disponível em:

```text
docs/
└── arquitetura-menufast.png
```

<p align="center">
  <img src="arquitetura-img.png" alt="Diagrama de arquitetura do MenuFast">
</p>

---

# 📌 Status do projeto

🚧 **Em desenvolvimento**

O projeto está em evolução e novos módulos, integrações e funcionalidades podem ser adicionados conforme a necessidade da plataforma.

---

# 👨‍💻 MenuFast

Sistema desenvolvido para centralizar a operação de estabelecimentos alimentícios, desde o atendimento e abertura do pedido até a produção na cozinha, fechamento no caixa e geração de informações gerenciais.
