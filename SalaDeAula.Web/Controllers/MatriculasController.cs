using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalaDeAula.Web.Models;
using System.Text;

namespace SalaDeAula.Web.Controllers
{
    public class MatriculasController : Controller
    {
        private readonly string _urlAPI = "https://localhost:44363/api"; // Ajuste sua porta!

        // GET: Carrega a tela. Se passar um ID, carrega os dados daquela turma.
        public async Task<IActionResult> Index(int? turmaId)
        {
            var viewModel = new MatriculaViewModel();

            using (var client = new HttpClient())
            {
                // 1. Busca todas as turmas para o primeiro Dropdown
                var resTurmas = await client.GetStringAsync($"{_urlAPI}/Turmas");
                viewModel.ListaTurmas = JsonConvert.DeserializeObject<List<TurmaViewModel>>(resTurmas);

                // 2. Busca todos os alunos para o Dropdown de "Adicionar"
                var resAlunos = await client.GetStringAsync($"{_urlAPI}/Alunos");
                viewModel.ListaAlunosDisponiveis = JsonConvert.DeserializeObject<List<AlunoViewModel>>(resAlunos);

                // 3. Se uma turma foi selecionada, busca quem está matriculado nela
                if (turmaId.HasValue)
                {
                    viewModel.TurmaSelecionadaId = turmaId.Value;

                    var resMatriculados = await client.GetStringAsync($"{_urlAPI}/AlunoTurma/{turmaId}");
                    viewModel.AlunosMatriculados = JsonConvert.DeserializeObject<List<AlunoViewModel>>(resMatriculados);
                }
            }

            return View(viewModel);
        }

        // POST: Faz a matrícula
        [HttpPost]
        public async Task<IActionResult> Vincular(MatriculaViewModel model)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                    AlunoId = model.AlunoParaAdicionarId,
                    TurmaId = model.TurmaSelecionadaId
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_urlAPI}/AlunoTurma", content);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    TempData["Erro"] = erro; // Mostra erro na tela (ex: Já matriculado)
                }
            }
            // Recarrega a página da mesma turma
            return RedirectToAction("Index", new { turmaId = model.TurmaSelecionadaId });
        }

        // POST: Remove a matrícula
        public async Task<IActionResult> Remover(int alunoId, int turmaId)
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync($"{_urlAPI}/AlunoTurma?alunoId={alunoId}&turmaId={turmaId}");
            }
            return RedirectToAction("Index", new { turmaId });
        }
    }
}