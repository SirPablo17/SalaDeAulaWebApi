using Dapper;
using System.Data.SqlClient;
using SalaDeAulaWebApi.Models; // Importante para reconhecer a classe Aluno

public class AlunoTurmaRepository : IAlunoTurmaRepository
{
    private readonly string _connectionString;
    public AlunoTurmaRepository(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<bool> ExisteVinculo(int alunoId, int turmaId)
    {
        using var conn = new SqlConnection(_connectionString);
        var existe = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM aluno_turma WHERE aluno_id = @alunoId AND turma_id = @turmaId",
            new { alunoId, turmaId });
        return existe > 0;
    }

    public async Task Vincular(int alunoId, int turmaId)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            "INSERT INTO aluno_turma (aluno_id, turma_id) VALUES (@alunoId, @turmaId)",
            new { alunoId, turmaId });
    }

    public async Task Desvincular(int alunoId, int turmaId)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            "DELETE FROM aluno_turma WHERE aluno_id = @alunoId AND turma_id = @turmaId",
            new { alunoId, turmaId });
    }

    // --- NOVO MÉTODO (ADICIONE ISSO) ---
    public async Task<IEnumerable<Aluno>> ObterAlunosPorTurma(int turmaId)
    {
        using var conn = new SqlConnection(_connectionString);
        var sql = @"
            SELECT a.id, a.nome, a.usuario, a.ativo 
            FROM aluno a
            INNER JOIN aluno_turma at ON a.id = at.aluno_id
            WHERE at.turma_id = @turmaId";

        return await conn.QueryAsync<Aluno>(sql, new { turmaId });
    }
}