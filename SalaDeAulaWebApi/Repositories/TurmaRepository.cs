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

    public async Task<Turma> ObterPorId(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        // Mapeamos 'turma' para 'NomeTurma' e 'curso_id' para 'CursoId'
        return await conn.QueryFirstOrDefaultAsync<Turma>(
            "SELECT id, curso_id as CursoId, turma as NomeTurma, ano FROM turma WHERE id = @id",
            new { id });
    }

    public async Task Atualizar(Turma turma)
    {
        using var conn = new SqlConnection(_connectionString);
        var sql = "UPDATE turma SET curso_id = @CursoId, turma = @NomeTurma, ano = @Ano WHERE id = @Id";
        await conn.ExecuteAsync(sql, turma);
    }

    // Verifica duplicidade ignorando o próprio ID (usado na edição)
    public async Task<bool> ExisteTurmaComNome(string nome, int excetoId)
    {
        using var conn = new SqlConnection(_connectionString);
        var existe = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM turma WHERE turma = @nome AND id <> @excetoId",
            new { nome, excetoId });
        return existe > 0;
    }
}