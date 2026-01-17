using System.ComponentModel.DataAnnotations;

namespace SalaDeAula.Web.Models
{
    public class MatriculaViewModel
    {
        // Qual turma estamos gerenciando agora?
        public int TurmaSelecionadaId { get; set; }

        // Qual aluno queremos adicionar?
        public int AlunoParaAdicionarId { get; set; }

        // Listas para preencher os Dropdowns (Selects)
        public List<TurmaViewModel> ListaTurmas { get; set; } = new List<TurmaViewModel>();
        public List<AlunoViewModel> ListaAlunosDisponiveis { get; set; } = new List<AlunoViewModel>();

        // Lista de alunos que JÁ estão na turma (para a tabela)
        public List<AlunoViewModel> AlunosMatriculados { get; set; } = new List<AlunoViewModel>();
    }
}