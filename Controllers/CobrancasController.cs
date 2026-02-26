using PlanoDePagamento.DTOs;
using PlanoDePagamento.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlanoDePagamento.Controllers;

[ApiController]
[Route("api/responsaveis/{responsavelId}/cobrancas")]
public class CobrancasController : ControllerBase
{
    private readonly CobrancaService _cobrancaService;
    private readonly PlanoDePagamentoService _planService;

    public CobrancasController(CobrancaService cobrancaService, PlanoDePagamentoService planService)
    {
        _cobrancaService = cobrancaService;
        _planService = planService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CobrancaDto>>> GetByResponsavel(int responsavelId)
    {
        var resultado = await _planService.GetCobrancasByResponsavelAsync(responsavelId);
        return Ok(resultado);
    }

    [HttpGet("quantidade")]
    public async Task<ActionResult<object>> GetQuantidade(int responsavelId)
    {
        var quantidade = await _planService.GetQuantidadeCobrancasResponsavelAsync(responsavelId);
        return Ok(new { Quantidade = quantidade });
    }

    [HttpPost("{cobrancaId}/pagamentos")]
    public async Task<ActionResult<CobrancaDto>> RegistrarPagamento(int responsavelId, int cobrancaId, RegistrarPagamentoDto dto)
    {
        try
        {
            var resultado = await _cobrancaService.RegistrarPagamentoAsync(cobrancaId, dto);
            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{cobrancaId}/cancelar")]
    public async Task<ActionResult<CobrancaDto>> Cancelar(int responsavelId, int cobrancaId)
    {
        try
        {
            var resultado = await _cobrancaService.CancelarAsync(cobrancaId);
            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
