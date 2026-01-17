using Dapper;
using System.Data.SqlClient;
using SalaDeAulaWebApi.Models;

public class TurmaRepository : ITurmaRepository
{
    private readonly string _connectionString;
    public TurmaRepository(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<bool> ExisteTurmaComNome(string nome)
    {
        using var conn = new SqlConnection(_connectionString);
        var existe = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM turma WHERE turma = @nome", new { nome });
        return existe > 0;
    }

    public async Task<int> Adicionar(Turma turma)
    {
        using var conn = new SqlConnection(_connectionString);
        var sql = "INSERT INTO turma (curso_id, turma, ano) VALUES (@CursoId, @NomeTurma, @Ano)";
        return await conn.ExecuteAsync(sql, turma);
    }

    public async Task<IEnumerable<Turma>> ObterTodas()
    {
        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<Turma>("SELECT id, curso_id as CursoId, turma as NomeTurma, ano FROM turma");
    }

    public async Task Inativar(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync("DELETE FROM turma WHERE id = @id", new { id });
    }
}