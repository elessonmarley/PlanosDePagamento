using PlanoDePagamento.Enums;

namespace PlanoDePagamento.DTOs;

public class CobrancaDto
{ 
    public int Id { get; set; }
    public int PlanoDePagamentoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public StatusCobranca Status { get; set; }
    public string CodigoPagamento { get; set; } = string.Empty;
    public bool Vencida { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
