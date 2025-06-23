# 📘 Cliente API – Teste Técnico

API REST desenvolvida em **ASP.NET Core 8** com **Entity Framework In-Memory**, aplicando os princípios de **Domain-Driven Design (DDD)**. O sistema permite o cadastro e gerenciamento de clientes e seus respectivos endereços.

---

## 🖥️ Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## ✅ Funcionalidades

- `GET /api/v1/clientes` – Listar todos os clientes  
- `GET /api/v1/clientes/{id}` – Buscar cliente por ID  
- `POST /api/v1/clientes` – Criar um novo cliente  
- `PUT /api/v1/clientes/{id}` – Atualizar um cliente existente  
- `DELETE /api/v1/clientes/{id}` – Remover cliente  

---

## 🧱 Tecnologias Utilizadas

- ASP.NET Core 8 Web API  
- Entity Framework Core In-Memory  
- AutoMapper  
- xUnit + Moq (Testes unitários)  
- Swagger (Swashbuckle)  
- API Versioning (`Microsoft.AspNetCore.Mvc.Versioning`)  
- Injeção de Dependência  
- ILogger (Logs estruturados)  

---

## 📁 Estrutura do Projeto

```
ClienteApi/
├── Controllers       → Endpoints da API
├── Data              → DbContext (EF In-Memory)
├── DTOs              → Objetos de transferência de dados
├── Extensions        → AutoMapper, Versionamento, DI
├── Models            → Entidades de domínio
├── Repositories      → Interfaces e implementações do repositório
├── Services          → Lógica de aplicação
├── Program.cs        → Configuração da API
```

---

## 🚀 Como Executar o Projeto

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/danielfk11/Desafio-eLaw.git
   cd ClienteApi
   ```

2. **Restaurar e rodar a aplicação:**
   ```bash
   dotnet restore
   dotnet run
   ```

> **Nota:** Ao executar pelo Visual Studio, certifique-se de selecionar o perfil **IIS Express** para iniciar a aplicação.

3. **Acessar a documentação Swagger:**
   - [https://localhost:7023/swagger](https://localhost:7023/swagger)
   - ou [http://localhost:5186/swagger](http://localhost:5186/swagger)

> Por padrão, a API utiliza a rota base: `https://localhost:7023/api/v1/clientes` ou `http://localhost:5186/api/v1/clientes`

---

## ✅ Como Executar os Testes

1. Acesse a pasta do projeto de testes:
   ```bash
   cd ClienteApi.Tests
   ```

2. Restaure as dependências:
   ```bash
   dotnet restore
   ```

3. Execute os testes:
   ```bash
   dotnet test
   ```

---

## 🧪 Exemplo de JSON para criar cliente

```json
{
  "nome": "Daniel Kiffer",
  "email": "daniel@email.com",
  "telefone": "21999999999",
  "endereco": {
    "rua": "Rua A",
    "numero": "123",
    "cidade": "Rio de Janeiro",
    "estado": "RJ",
    "cep": "01000-000"
  }
}
```
