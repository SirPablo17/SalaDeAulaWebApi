using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalaDeAula.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalaDeAula.Web.Controllers
{
    public class AlunosController : Controller
    {
        // ⚠️ IMPORTANTE: Troque esta porta pela porta onde sua API está rodando!
        // Você vê a porta na URL do Swagger (ex: localhost:7047)
        private readonly string _baseUrl = "https://localhost:44363/api/Alunos";

        public async Task<IActionResult> Index()
        {
            List<AlunoViewModel> listaAlunos = new List<AlunoViewModel>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_baseUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        // Transforma o JSON da API em lista de objetos C#
                        listaAlunos = JsonConvert.DeserializeObject<List<AlunoViewModel>>(apiResponse);
                    }
                }
            }

            return View(listaAlunos);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Recebe os dados e envia para a API
        [HttpPost]
        public async Task<IActionResult> Create(AlunoViewModel aluno)
        {
            if (!ModelState.IsValid)
                return View(aluno);

            using (var httpClient = new HttpClient())
            {
                // Transforma o objeto em JSON para enviar
                var content = new StringContent(JsonConvert.SerializeObject(aluno), System.Text.Encoding.UTF8, "application/json");

                // Envia o POST para a API (A mesma URL da listagem)
                using (var response = await httpClient.PostAsync(_baseUrl, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // Se deu certo, volta para a lista
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Se deu erro (ex: senha fraca), mostra o erro na tela
                        string erro = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"Erro da API: {erro}");
                    }
                }
            }
            return View(aluno);
        }


        // GET: Abre a tela preenchida
        public async Task<IActionResult> Edit(int id)
        {
            using (var httpClient = new HttpClient())
            {
                // Busca o aluno pelo ID para preencher os campos
                using (var response = await httpClient.GetAsync($"{_baseUrl}/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var aluno = JsonConvert.DeserializeObject<AlunoViewModel>(apiResponse);
                        return View(aluno);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Envia as alterações
        [HttpPost]
        public async Task<IActionResult> Edit(AlunoViewModel aluno)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(aluno), System.Text.Encoding.UTF8, "application/json");

                // Note que usamos PutAsync para atualização
                using (var response = await httpClient.PutAsync($"{_baseUrl}/{aluno.Id}", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(aluno);
        }

        // --- BLOCO DE EXCLUSÃO (INATIVAR) ---

        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                // Chama o método Delete da API
                using (var response = await httpClient.DeleteAsync($"{_baseUrl}/{id}"))
                {
                    // Opcional: Adicionar mensagem de sucesso ou erro
                }
            }
            return RedirectToAction("Index");
        }
    }
}