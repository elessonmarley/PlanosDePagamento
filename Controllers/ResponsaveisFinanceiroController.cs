using PlanoDePagamento.DTOs;
using PlanoDePagamento.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlanoDePagamento.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponsaveisFinanceiroController : ControllerBase
{
    private readonly ResponsavelFinanceiroService _service;

    public ResponsaveisFinanceiroController(ResponsavelFinanceiroService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ResponsavelFinanceiroDto>> Create(CreateResponsavelFinanceiroDto dto)
    {
        var resultado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = resultado?.Id }, resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponsavelFinanceiroDto>> GetById(int id)
    {
        var resultado = await _service.GetByIdAsync(id);
        if (resultado == null)
            return NotFound();
        return Ok(resultado);
    }

    [HttpGet]
    public async Task<ActionResult<List<ResponsavelFinanceiroDto>>> GetAll()
    {
        var resultado = await _service.GetAllAsync();
        return Ok(resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponsavelFinanceiroDto>> Update(int id, CreateResponsavelFinanceiroDto dto)
    {
        var resultado = await _service.UpdateAsync(id, dto);
        if (resultado == null)
            return NotFound();
        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var sucesso = await _service.DeleteAsync(id);
        if (!sucesso)
            return NotFound();
        return NoContent();
    }
}
