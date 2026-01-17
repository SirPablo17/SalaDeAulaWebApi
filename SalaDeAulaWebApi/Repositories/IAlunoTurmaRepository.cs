public interface IAlunoTurmaRepository
{
    Task Vincular(int alunoId, int turmaId);
    Task<bool> ExisteVinculo(int alunoId, int turmaId); // Regra de Negócio 2
    Task Desvincular(int alunoId, int turmaId);
}