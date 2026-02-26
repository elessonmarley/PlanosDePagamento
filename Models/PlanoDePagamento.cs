namespace PlanoDePagamento.Models;

/// <summary>
/// Plano de pagamento vinculado a um responsável financeiro e um centro de custo
/// Contém um conjunto de cobranças
/// </summary>
public class PlanoDePagamento
{
    public int Id { get; set; }
    public int ResponsavelFinanceiroId { get; set; }
    public int CentroDeCustoId { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    // Navigation
    public ResponsavelFinanceiro? ResponsavelFinanceiro { get; set; }
    public CentroDeCusto? CentroDeCusto { get; set; }
    public ICollection<Cobranca> Cobrancas { get; set; } = new List<Cobranca>();
}
