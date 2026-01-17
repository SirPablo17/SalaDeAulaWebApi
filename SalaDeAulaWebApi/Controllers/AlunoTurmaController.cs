using Microsoft.AspNetCore.Mvc;
using SalaDeAulaWebApi.Repositories;

namespace SalaDeAulaWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoTurmaController : ControllerBase
    {
        private readonly IAlunoTurmaRepository _repository;

        public AlunoTurmaController(IAlunoTurmaRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Vincular([FromBody] MatriculaDto request)
        {
            // Regra de Negócio 2: Não permitir duplicidade
            if (await _repository.ExisteVinculo(request.AlunoId, request.TurmaId))
            {
                return BadRequest("Este aluno já está vinculado a esta turma.");
            }

            await _repository.Vincular(request.AlunoId, request.TurmaId);
            return Ok("Vinculado com sucesso!");
        }

        [HttpDelete]
        public async Task<IActionResult> Desvincular(int alunoId, int turmaId)
        {
            await _repository.Desvincular(alunoId, turmaId);
            return Ok("Desvinculado.");
        }

        [HttpGet("{turmaId}")]
        public async Task<IActionResult> ListarAlunos(int turmaId)
        {
            var alunos = await _repository.ObterAlunosPorTurma(turmaId);
            return Ok(alunos);
        }
    }

    // DTO para receber o JSON
    public class MatriculaDto
    {
        public int AlunoId { get; set; }
        public int TurmaId { get; set; }
    }
}