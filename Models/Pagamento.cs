namespace PlanoDePagamento.Models;

/// <summary>
/// Registro de recebimento de uma cobrança específica
/// </summary>
public class Pagamento
{
    public int Id { get; set; }
    public int CobrancaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime CriadoEm { get; set; }

    // Navigation
    public Cobranca? Cobranca { get; set; }
}
