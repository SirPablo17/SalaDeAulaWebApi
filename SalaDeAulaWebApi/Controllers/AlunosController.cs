using Microsoft.AspNetCore.Mvc;
using SalaDeAulaWebApi.Models;
using SalaDeAulaWebApi.Repositories;


[ApiController]
[Route("api/[controller]")]
public class AlunosController : ControllerBase
{
    private readonly IAlunoRepository _repository;

    public AlunosController(IAlunoRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> CriarAluno(Aluno aluno)
    {
        // 1. Regra de Negócio: Validar senha forte (ex: min 6 caracteres)
        if (aluno.Senha.Length < 6)
            return BadRequest("A senha deve ter pelo menos 6 caracteres.");

        // 2. Regra de Negócio: Transformar senha em HASH antes de salvar
        aluno.Senha = BCrypt.Net.BCrypt.HashPassword(aluno.Senha);

        await _repository.Adicionar(aluno);
        return Ok("Aluno cadastrado com sucesso!");
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var alunos = await _repository.ObterTodos();
        return Ok(alunos);
    }

    // GET: api/Alunos/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var aluno = await _repository.ObterPorId(id);
        if (aluno == null) return NotFound();
        return Ok(aluno);
    }

    // PUT: api/Alunos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Aluno aluno)
    {
        if (id != aluno.Id) return BadRequest();

        // Regra: Hash de senha novamente se ela for alterada
        // (Num cenário real validaríamos se a senha mudou, aqui simplificaremos)
        if (!string.IsNullOrEmpty(aluno.Senha))
        {
            aluno.Senha = BCrypt.Net.BCrypt.HashPassword(aluno.Senha);
        }

        await _repository.Atualizar(aluno);
        return Ok("Aluno atualizado!");
    }
}