# Sistema de Gestão Escolar 🎓

Este é um projeto Full Stack desenvolvido em .NET para gerenciamento de alunos, turmas e matrículas escolares. A solução é composta por uma **API RESTful** robusta e uma aplicação **Web MVC** para interface com o usuário.

## 🚀 Tecnologias Utilizadas

* **.NET 6 / 7** (Core)
* **ASP.NET Core Web API** (Backend)
* **ASP.NET Core MVC** (Frontend)
* **Dapper** (Micro-ORM para alta performance)
* **SQL Server** (Banco de Dados)
* **Swagger** (Documentação da API)
* **Bootstrap 5** (Estilização das telas)
* **BCrypt.Net** (Segurança e Hash de Senhas)

## 📦 Principais Dependências (NuGet)

O projeto utiliza as seguintes bibliotecas para facilitar o desenvolvimento e garantir performance:

* **[Dapper](https://www.nuget.org/packages/Dapper/):** Micro-ORM utilizado para o mapeamento objeto-relacional e execução de queries SQL de alta performance.
* **[System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient/):** Provedor de dados para conexão robusta com o SQL Server.
* **[BCrypt.Net-Next](https://www.nuget.org/packages/BCrypt.Net-Next/):** Biblioteca para hashing de senhas seguro e validação de login.
* **[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/):** Utilizado no Front-End para serialização e deserialização dos dados vindos da API.
* **[Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/):** Ferramenta para gerar a documentação automática (Swagger) da API.

## ⚙️ Funcionalidades

### 👤 Alunos
* Cadastro, Edição e Listagem.
* **Inativação Lógica (Soft Delete):** O aluno não é excluído do banco, apenas inativado, preservando o histórico.
* **Segurança:** Senhas salvas criptografadas (Hash).
* **Validação:** Senhas fracas não são permitidas.

### 🏫 Turmas
* Cadastro e Gerenciamento de Turmas.
* **Regras de Negócio:**
    * Não permite nomes de turmas duplicados.
    * Não permite criação de turmas com ano anterior ao atual.

### 🔗 Matrículas (Relacionamento)
* Vínculo de Alunos em Turmas.
* **Regras de Negócio:**
    * Controle de duplicidade (não permite matricular o mesmo aluno duas vezes na mesma turma).
    * Visualização clara dos alunos matriculados por turma.

---

## 🛠️ Como Rodar o Projeto

### 1. Configuração do Banco de Dados
Certifique-se de ter o **SQL Server** instalado. Execute o script abaixo para criar o banco e as tabelas necessárias:

### 2. String de conexão
"ConnectionStrings": {
  "DefaultConnection": ""Server=PABLONASCIME1;Database=SalaDeAulaAPI;Trusted_Connection=True;TrustServerCertificate=True;""
}

```sql
CREATE DATABASE SalaDeAulaAPI;
GO
USE SalaDeAulaAPI;
GO

-- Tabela de Alunos
CREATE TABLE aluno (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    usuario VARCHAR(45) NOT NULL,
    senha CHAR(60) NOT NULL, -- Tamanho para Hash BCrypt
    ativo BIT DEFAULT 1 NOT NULL
);

-- Tabela de Turmas
CREATE TABLE turma (
    id INT IDENTITY(1,1) PRIMARY KEY,
    curso_id INT NOT NULL,
    turma VARCHAR(45) NOT NULL,
    ano INT NOT NULL
);

-- Tabela de Relacionamento (Matrícula)
CREATE TABLE aluno_turma (
    aluno_id INT NOT NULL,
    turma_id INT NOT NULL,
    PRIMARY KEY (aluno_id, turma_id),
    FOREIGN KEY (aluno_id) REFERENCES aluno(id),
    FOREIGN KEY (turma_id) REFERENCES turma(id)
);
