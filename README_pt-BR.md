# Plano de Pagamento - Solução Completa

Este repositório contém uma aplicação completa para gerenciamento de planos de pagamento e cobranças.

## 🚀 Quick Start

```bash
# 1. Install .NET 8.0 from https://dotnet.microsoft.com/download
# 2. Install PostgreSQL from https://www.postgresql.org/download/
# 3. Create database
createdb plano_de_pagamento

# 4. Navigate to project
cd PlanoDePagamento

# 5. Restore dependencies  
dotnet restore

# 6. Apply migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

# 7. Run the application
dotnet run

# Access API at http://localhost:5000/swagger
```

## 📦 Instalação Detalhada do PostgreSQL

### Windows

1. **Baixar o Instalador**
   - Acesse: https://www.postgresql.org/download/windows/
   - Clique em "Download the  installer"

2. **Executar o Instalador** 
   - Execute o arquivo `.exe`
   - Clique em "Next" para prosseguir

3. **Configurar Dados de Acesso**
   - **Username**: `postgres` (padrão, deixe como está)
   - **Password**: `123` 
   - **Confirm password**: `123`
   - Anote essas credenciais!

4. **Porta do Servidor**
   - **Port**: `5432` (padrão, deixe como está)

5. **Locale**
   - Deixe como padrão e continue

6. **Finalizar Instalação**
   - Clique em "Finish"
   - O PostgreSQL iniciará automaticamente

### Verificar Instalação

Abra PowerShell e execute:

```powershell
# Conectar ao PostgreSQL (pedirá senha: 123)
psql -U postgres

# Se conectar com sucesso, você verá o prompt:
# postgres=#

# Sair
\q
```

### Criar Banco de Dados

```powershell
# Opção 1: Usando createdb
createdb -U postgres plano_de_pagamento

# Opção 2: Usando psql
psql -U postgres -c "CREATE DATABASE plano_de_pagamento;"

# Senha quando solicitado: 123
```

### Verificar se o Banco Existe

```powershell
psql -U postgres -l

# Você deve ver a linha:
# plano_de_pagamento | postgres | ...
```

## ⚙️ Configuração da Connection String

O arquivo `appsettings.json` já está configurado com:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=plano_de_pagamento;Username=postgres;Password=123"
  }
}
```

Se precisar alterar a senha ou usuário, edite este arquivo.

## 📋 Estrutura do Projeto

- **Models**: Entidades de domínio (Responsável, Centro, Plano, Cobrança, Pagamento)
- **Services**: Lógica de negócio
- **Controllers**: Endpoints REST
- **DTOs**: Data Transfer Objects para API
- **Data**: Entity Framework DbContext e configurações
- **Enums**: Tipos enumerados (Método Pagamento, Status, etc)

## 🔌 API Endpoints

```
POST   /api/responsaveisfinanceiro              # Criar responsável
GET    /api/responsaveisfinanceiro              # Listar responsáveis
GET    /api/responsaveisfinanceiro/{id}         # Obter responsável

POST   /api/centrosdecusto                      # Criar centro de custo
GET    /api/centrosdecusto                      # Listar centros
GET    /api/centrosdecusto/{id}                 # Obter centro

POST   /api/planosdepagamento                   # Criar plano
GET    /api/planosdepagamento/{id}              # Obter plano
GET    /api/planosdepagamento/{id}/total        # Valor total
GET    /api/planosdepagamento/responsavel/{id}  # Planos por responsável

GET    /api/responsaveis/{id}/cobrancas         # Cobranças do responsável
GET    /api/responsaveis/{id}/cobrancas/quantidade  # Quantidade de cobranças
POST   /api/responsaveis/{id}/cobrancas/{cobId}/pagamentos  # Registrar pagamento
```

## 📝 Exemplo: Criar Plano Completo

```bash
# 1. Criar responsável
curl -X POST https://localhost:5001/api/responsaveisfinanceiro \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "identificador": "CPF123456789"
  }'
# Resposta: { "id": 1, ... }

# 2. Criar plano com cobranças
curl -X POST https://localhost:5001/api/planosdepagamento \
  -H "Content-Type: application/json" \
  -d '{
    "responsavelId": 1,
    "centroDeCustoId": 1,
    "cobrancas": [
      {
        "valor": 500.00,
        "dataVencimento": "2025-03-10",
        "metodoPagamento": "BOLETO"
      },
      {
        "valor": 500.00,
        "dataVencimento": "2025-04-10",
        "metodoPagamento": "PIX"
      }
    ]
  }'

# 3. Registrar pagamento
curl -X POST https://localhost:5001/api/responsaveis/1/cobrancas/1/pagamentos \
  -H "Content-Type: application/json" \
  -d '{
    "valor": 500.00,
    "dataPagamento": "2025-03-09"
  }'
```

## 🗄️ Database Schema

**ResponsaveisFinanceiros**
- Id (PK)
- Nome
- Identificador (UNIQUE)
- CriadoEm, AtualizadoEm

**CentrosDeCusto**
- Id (PK)
- Nome
- Descricao
- Ativo
- CriadoEm, AtualizadoEm

**PlanosDePagamento**
- Id (PK)
- ResponsavelFinanceiroId (FK)
- CentroDeCustoId (FK)
- ValorTotal
- CriadoEm, AtualizadoEm

**Cobrancas**
- Id (PK)
- PlanoDePagamentoId (FK)
- Valor
- DataVencimento
- MetodoPagamento (ENUM)
- Status (ENUM: EMITIDA, PAGA, CANCELADA)
- CodigoPagamento (UNIQUE)
- CriadoEm, AtualizadoEm

**Pagamentos**
- Id (PK)
- CobrancaId (FK)
- Valor
- DataPagamento
- CriadoEm

## ✨ Funcionalidades

✅ Cadastro de Responsáveis Financeiros
✅ Gerenciamento de Centros de Custo (customizável)
✅ Criação de Planos com Múltiplas Cobranças
✅ Registro e Rastreamento de Pagamentos
✅ Status de Cobrança com Vencimento Automático
✅ Geração Automática de Códigos (Boleto/PIX)
✅ Cálculo Automático de Totais
✅ API REST completa com Swagger

## 🔑 Tecnologias

- .NET 8.0
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI

## 📚 Documentação Completa

Veja `README.md` para documentação detalhada, exemplos com cURL, troubleshooting e próximos passos.
