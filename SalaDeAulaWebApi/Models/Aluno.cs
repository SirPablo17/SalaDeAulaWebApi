namespace SalaDeAulaWebApi.Models;

public class Aluno
{
    public int Id { get; set; } // [cite: 4]
    public string Nome { get; set; } = string.Empty;  // [cite: 9]
    public string Usuario { get; set; } = string.Empty; // [cite: 12]
    public string Senha { get; set; } = string.Empty; // [cite: 14]

    public bool Ativo { get; set; }
}
