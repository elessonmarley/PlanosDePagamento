using PlanoDePagamento.DTOs;
using PlanoDePagamento.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlanoDePagamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CentrosDeCustoController : ControllerBase
{
    private readonly CentroDeCustoService _service;

    public CentrosDeCustoController(CentroDeCustoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<CentroDeCustoDto>> Create(CreateCentroDeCustoDto dto)
    {
        var resultado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = resultado?.Id }, resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CentroDeCustoDto>> GetById(int id)
    {
        var resultado = await _service.GetByIdAsync(id);
        if (resultado == null)
            return NotFound();
        return Ok(resultado);
    }

    [HttpGet]
    public async Task<ActionResult<List<CentroDeCustoDto>>> GetAll([FromQuery] bool onlyActive = true)
    {
        var resultado = await _service.GetAllAsync(onlyActive);
        return Ok(resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CentroDeCustoDto>> Update(int id, CreateCentroDeCustoDto dto)
    {
        var resultado = await _service.UpdateAsync(id, dto);
        if (resultado == null)
            return NotFound();
        return Ok(resultado);
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<ActionResult> Deactivate(int id)
    {
        var sucesso = await _service.DeactivateAsync(id);
        if (!sucesso)
            return NotFound();
        return NoContent();
    }
}
