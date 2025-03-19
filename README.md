# Travel Routes API

API para gerenciamento de rotas de viagem e busca do melhor preço entre destinos.

## 🚀 Tecnologias Utilizadas

- **.NET 8.0**
- **ASP.NET Core**
- **Dapper**
- **SQLite**
- **AutoMapper**
- **Swagger/OpenAPI**
- **FluentValidation**
- **xUnit**
- **Moq**
- **FluentAssertions**

## 📋 Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Um editor de código (recomendado: Visual Studio Code ou Visual Studio 2022)

## 🔧 Configuração do Ambiente

1. Clone o repositório:
```bash
git clone https://github.com/Robertoscj/DesafioINDT.git
cd [nome-do-repositorio]
```

2. Navegue até o diretório do projeto:
```bash
cd src/TravelRoutes.API
```

3. Restaure as dependências:
```bash
dotnet restore
```

4. Execute as migrações do banco de dados:
```bash
dotnet ef database update
```

## 🚀 Executando a API

1. No diretório src/TravelRoutes.API, execute:
```bash
dotnet run
```

2. A API estará disponível em:
- http://localhost:5101

3. A documentação Swagger estará disponível em:
- http://localhost:5101

## 📝 Endpoints Disponíveis

### Rotas
- **GET /api/routes** - Lista todas as rotas
- **GET /api/routes/{id}** - Obtém uma rota específica
- **POST /api/routes** - Cria uma nova rota
- **PUT /api/routes/{id}** - Atualiza uma rota existente
- **DELETE /api/routes/{id}** - Remove uma rota
- **GET /api/routes/cheapest** - Encontra a rota mais barata entre origem e destino

## 🧪 Executando os Testes

Para executar os testes unitários e de integração:

```bash
cd src/TravelRoutes.Tests
dotnet test
```

## 📚 Estrutura do Projeto

O projeto segue a arquitetura limpa (Clean Architecture) e está organizado nos seguintes projetos:

- **TravelRoutes.API**: Camada de apresentação com controllers e configurações da API
- **TravelRoutes.Application**: Camada de aplicação com serviços, DTOs e validações
- **TravelRoutes.Domain**: Camada de domínio com entidades e interfaces
- **TravelRoutes.Infrastructure**: Camada de infraestrutura com implementações de repositórios
- **TravelRoutes.Tests**: Projeto de testes unitários e de integração

## 🔒 Segurança

A API implementa:
- Rate Limiting para proteção contra excesso de requisições
- Validação de entrada com FluentValidation
- Tratamento global de exceções
- Logs de operações

## 📖 Documentação Adicional

A documentação completa da API está disponível através do Swagger UI, que pode ser acessado ao executar a aplicação e navegar para http://localhost:5101


## ✨ Funcionalidades Principais

- Cadastro e gerenciamento de rotas
- Busca da rota mais barata entre dois pontos
- Validação de dados de entrada
- Documentação interativa via Swagger
- Testes automatizados
- Tratamento de erros padronizado 