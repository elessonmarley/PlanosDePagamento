namespace PlanoDePagamento.DTOs;

public class PagamentoDto
{
    public int Id { get; set; }
    public int CobrancaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime CriadoEm { get; set; }
}
