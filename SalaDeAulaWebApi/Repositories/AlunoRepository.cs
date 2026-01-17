using Dapper;
using System.Data.SqlClient; // Mantemos o driver clássico que funcionou
using SalaDeAulaWebApi.Models;

namespace SalaDeAulaWebApi.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly string _connectionString;

        // Injetamos a Configuração para ler do arquivo appsettings.json
        public AlunoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Adicionar(Aluno aluno)
        {
            // O "using" aqui garante que a conexão abra e feche corretamente a cada chamada
            using var conn = new SqlConnection(_connectionString);

            var sql = "INSERT INTO aluno (nome, usuario, senha, ativo) VALUES (@Nome, @Usuario, @Senha, 1)";

            return await conn.ExecuteAsync(sql, aluno);
        }

        public async Task<IEnumerable<Aluno>> ObterTodos()
        {
            using var conn = new SqlConnection(_connectionString);
            // Removemos o 'WHERE ativo = 1' para listar todos e poder ver o status na tela
            return await conn.QueryAsync<Aluno>("SELECT id, nome, usuario, ativo FROM aluno");
        }

        public async Task<Aluno> ObterPorId(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Aluno>("SELECT * FROM aluno WHERE id = @id", new { id });
        }

        public async Task Atualizar(Aluno aluno)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "UPDATE aluno SET nome = @Nome, usuario = @Usuario, senha = @Senha WHERE id = @Id";
            await conn.ExecuteAsync(sql, aluno);
        }

        public async Task Inativar(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            // Em vez de deletar, mudamos o status para 0 (false)
            var sql = "UPDATE aluno SET ativo = 0 WHERE id = @id";
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task Ativar(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            // A única diferença é o ativo = 1
            var sql = "UPDATE aluno SET ativo = 1 WHERE id = @id";
            await conn.ExecuteAsync(sql, new { id });
        }
    }
}