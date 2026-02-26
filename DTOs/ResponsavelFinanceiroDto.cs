namespace PlanoDePagamento.DTOs;

public class ResponsavelFinanceiroDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Identificador { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
