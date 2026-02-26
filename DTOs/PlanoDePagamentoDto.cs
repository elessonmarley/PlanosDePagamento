namespace PlanoDePagamento.DTOs;

public class PlanoDePagamentoDto
{
    public int Id { get; set; }
    public int ResponsavelFinanceiroId { get; set; }
    public int CentroDeCustoId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CobrancaDto> Cobrancas { get; set; } = new List<CobrancaDto>();
    public ResponsavelFinanceiroDto? ResponsavelFinanceiro { get; set; }
    public CentroDeCustoDto? CentroDeCusto { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
