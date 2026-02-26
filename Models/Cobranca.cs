using PlanoDePagamento.Enums;

namespace PlanoDePagamento.Models;

/// <summary>
/// Cobrança (parcela) vinculada a um plano de pagamento
/// </summary>
public class Cobranca
{
    public int Id { get; set; }
    public int PlanoDePagamentoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public StatusCobranca Status { get; set; } = StatusCobranca.EMITIDA;
    public string CodigoPagamento { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    // Navigation
    public PlanoDePagamento? PlanoDePagamento { get; set; }
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    /// <summary>
    /// Determina se a cobrança está vencida (data atual > data de vencimento e não está PAGA nem CANCELADA)
    /// </summary>
    public bool EstaVencida()
    {
        return DateTime.UtcNow > DataVencimento && 
               Status != StatusCobranca.PAGA && 
               Status != StatusCobranca.CANCELADA;
    }
}
