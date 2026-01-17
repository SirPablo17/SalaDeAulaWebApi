using SalaDeAulaWebApi.Models;

public interface IAlunoTurmaRepository
{
    Task Vincular(int alunoId, int turmaId);
    Task<bool> ExisteVinculo(int alunoId, int turmaId);
    Task Desvincular(int alunoId, int turmaId);

    // --- ADICIONE ESTA LINHA ---
    // Precisamos disso para mostrar a lista de chamada na tela
    Task<IEnumerable<Aluno>> ObterAlunosPorTurma(int turmaId);
}