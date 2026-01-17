using Microsoft.AspNetCore.Mvc;
using SalaDeAulaWebApi.Models;

[ApiController]
[Route("api/[controller]")]
public class TurmasController : ControllerBase
{
    private readonly ITurmaRepository _repository;
    public TurmasController(ITurmaRepository repository) => _repository = repository;

    [HttpPost]
    public async Task<IActionResult> Post(Turma turma)
    {
        if (turma.Ano < DateTime.Now.Year)
            return BadRequest("O ano da turma não pode ser anterior ao ano atual.");

        if (await _repository.ExisteTurmaComNome(turma.NomeTurma))
            return BadRequest("Já existe uma turma cadastrada com este nome.");

        await _repository.Adicionar(turma);
        return Ok("Turma criada com sucesso!");
    }
}