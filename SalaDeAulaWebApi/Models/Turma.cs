namespace SalaDeAulaWebApi.Models;

public class Turma
{
    public int Id { get; set; } // [cite: 5]
    public int CursoId { get; set; } // [cite: 10]
    public string NomeTurma { get; set; } = string.Empty; // [cite: 13]
    public int Ano { get; set; } // [cite: 15]
}