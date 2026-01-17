using SalaDeAulaWebApi.Models;

namespace SalaDeAulaWebApi.Repositories;

public interface IAlunoRepository
{
    Task<IEnumerable<Aluno>> ObterTodos();
    Task<Aluno> ObterPorId(int id);
    Task<int> Adicionar(Aluno aluno);
    Task Atualizar(Aluno aluno);
    Task Inativar(int id); // Requisito: Inativar Aluno [cite: 38]
}