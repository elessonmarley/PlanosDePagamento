namespace PlanoDePagamento.Models;

/// <summary>
/// Centro de custo que define a natureza do plano de pagamento (MATRICULA, MENSALIDADE, MATERIAL, etc.)
/// </summary>
public class CentroDeCusto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    // Navigation
    public ICollection<PlanoDePagamento> PlanosDePagamento { get; set; } = new List<PlanoDePagamento>();
}
