namespace PlanoDePagamento.Models;

/// <summary>
/// Responsável financeiro pela gestão dos planos de pagamento
/// </summary>
public class ResponsavelFinanceiro
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Identificador { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    // Navigation
    public ICollection<PlanoDePagamento> PlanosDePagamento { get; set; } = new List<PlanoDePagamento>();
}
