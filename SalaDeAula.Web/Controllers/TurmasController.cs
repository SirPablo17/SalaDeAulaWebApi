using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalaDeAula.Web.Models;
using System.Text;

namespace SalaDeAula.Web.Controllers
{
    public class TurmasController : Controller
    {
        // ⚠️ ATENÇÃO: Verifique a porta da sua API aqui também!
        private readonly string _baseUrl = "https://localhost:44363/api/Turmas";

        public async Task<IActionResult> Index()
        {
            var turmas = new List<TurmaViewModel>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_baseUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    turmas = JsonConvert.DeserializeObject<List<TurmaViewModel>>(json);
                }
            }
            return View(turmas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TurmaViewModel turma)
        {
            if (!ModelState.IsValid) return View(turma);

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(turma);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_baseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // AQUI CAPTURAMOS O ERRO DA REGRA DE NEGÓCIO (Ano inválido ou Nome duplicado)
                    var erro = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Erro ao salvar: {erro}");
                    return View(turma);
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync($"{_baseUrl}/{id}");
            }
            return RedirectToAction("Index");
        }

        // GET: Abre a tela de edição preenchida
        public async Task<IActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_baseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var turma = JsonConvert.DeserializeObject<TurmaViewModel>(json);
                    return View(turma);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Envia a edição
        [HttpPost]
        public async Task<IActionResult> Edit(TurmaViewModel turma)
        {
            if (!ModelState.IsValid) return View(turma);

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(turma);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Note o PUT aqui
                var response = await client.PutAsync($"{_baseUrl}/{turma.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Erro ao atualizar: {erro}");
                    return View(turma);
                }
            }
        }
    }
}