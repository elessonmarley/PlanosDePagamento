using PlanoDePagamento.Enums;

namespace PlanoDePagamento.DTOs;

public class CreateCobrancaDto
{
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
}
