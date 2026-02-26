using PlanoDePagamento.DTOs;
using PlanoDePagamento.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlanoDePagamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanosDePagamentoController : ControllerBase
{
    private readonly PlanoDePagamentoService _planService;
    private readonly CobrancaService _cobrancaService;

    public PlanosDePagamentoController(PlanoDePagamentoService planService, CobrancaService cobrancaService)
    {
        _planService = planService;
        _cobrancaService = cobrancaService;
    }

    [HttpPost]
    public async Task<ActionResult<PlanoDePagamentoDto>> Create(CreatePlanoDePagamentoDto dto)
    {
        try
        {
            var resultado = await _planService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = resultado?.Id }, resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlanoDePagamentoDto>> GetById(int id)
    {
        var resultado = await _planService.GetByIdAsync(id);
        if (resultado == null)
            return NotFound();
        return Ok(resultado);
    }

    [HttpGet("{id}/total")]
    public async Task<ActionResult<object>> GetTotal(int id)
    {
        var total = await _planService.GetTotalAsync(id);
        if (total == 0M && await _planService.GetByIdAsync(id) == null)
            return NotFound();
        return Ok(new { ValorTotal = total });
    }

    [HttpGet("responsavel/{responsavelId}")]
    public async Task<ActionResult<List<PlanoDePagamentoDto>>> GetByResponsavel(int responsavelId)
    {
        var resultado = await _planService.GetByResponsavelAsync(responsavelId);
        return Ok(resultado);
    }
}
