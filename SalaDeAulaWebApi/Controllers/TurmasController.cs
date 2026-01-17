using Microsoft.AspNetCore.Mvc;
using SalaDeAulaWebApi.Models;
using SalaDeAulaWebApi.Repositories;

namespace SalaDeAulaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaRepository _repository;

        public TurmasController(ITurmaRepository repository)
        {
            _repository = repository;
        }

        // --- ESTE ERA O MÉTODO QUE FALTAVA ---
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var turmas = await _repository.ObterTodas();
            return Ok(turmas);
        }
        // -------------------------------------

        [HttpPost]
        public async Task<IActionResult> Post(Turma turma)
        {
            // Regra 4: Validação de ano
            if (turma.Ano < DateTime.Now.Year)
                return BadRequest("O ano da turma não pode ser anterior ao ano atual.");

            // Regra 1: Validação de nome único
            if (await _repository.ExisteTurmaComNome(turma.NomeTurma))
                return BadRequest("Já existe uma turma cadastrada com este nome.");

            await _repository.Adicionar(turma);
            return Ok("Turma criada com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Inativar(id);
            return Ok("Turma removida.");
        }

        // GET: api/Turmas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var turma = await _repository.ObterPorId(id);
            if (turma == null) return NotFound();
            return Ok(turma);
        }

        // PUT: api/Turmas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Turma turma)
        {
            if (id != turma.Id) return BadRequest();

            // Regra 4: Ano não pode ser anterior ao atual
            if (turma.Ano < DateTime.Now.Year)
                return BadRequest("O ano da turma não pode ser anterior ao ano atual.");

            // Regra 1: Validação de nome único (excluindo o ID atual)
            if (await _repository.ExisteTurmaComNome(turma.NomeTurma, id))
                return BadRequest("Já existe outra turma cadastrada com este nome.");

            await _repository.Atualizar(turma);
            return Ok("Turma atualizada!");
        }
    }
}