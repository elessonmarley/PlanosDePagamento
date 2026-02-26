namespace PlanoDePagamento.DTOs;

public class CreatePlanoDePagamentoDto
{
    public int ResponsavelId { get; set; }
    public int CentroDeCustoId { get; set; }
    public List<CreateCobrancaDto> Cobrancas { get; set; } = new List<CreateCobrancaDto>();
}
