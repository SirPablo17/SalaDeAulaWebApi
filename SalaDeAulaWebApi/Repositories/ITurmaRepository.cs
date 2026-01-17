using SalaDeAulaWebApi.Models;

public interface ITurmaRepository
{
    Task<IEnumerable<Turma>> ObterTodas();
    Task<int> Adicionar(Turma turma);
    Task Inativar(int id);
    Task<bool> ExisteTurmaComNome(string nome); // Necessário para a Regra 1
}