namespace GestaoClientes.API.Entities;

public class Cliente
{
    public int Id { get; set; } // Chave Primaria (PK)
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public bool ativo { get; set; } = true;
}