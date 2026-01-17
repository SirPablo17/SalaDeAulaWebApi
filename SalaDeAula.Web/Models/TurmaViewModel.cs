using System.ComponentModel.DataAnnotations;

namespace SalaDeAula.Web.Models
{
    public class TurmaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O ID do curso é obrigatório")]
        [Display(Name = "Código do Curso")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "O nome da turma é obrigatório")]
        [Display(Name = "Nome da Turma")]
        public string NomeTurma { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório")]
        public int Ano { get; set; }
    }
}