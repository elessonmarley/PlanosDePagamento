using PlanoDePagamento.Data;
using PlanoDePagamento.DTOs;
using PlanoDePagamento.Models;
using Microsoft.EntityFrameworkCore;

namespace PlanoDePagamento.Services;

public class CentroDeCustoService
{
    private readonly AppDbContext _context;

    public CentroDeCustoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CentroDeCustoDto?> CreateAsync(CreateCentroDeCustoDto dto)
    {
        var centro = new CentroDeCusto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Ativo = true,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };

        _context.CentrosDeCusto.Add(centro);
        await _context.SaveChangesAsync();

        return MapToDto(centro);
    }

    public async Task<CentroDeCustoDto?> GetByIdAsync(int id)
    {
        var centro = await _context.CentrosDeCusto.FindAsync(id);
        return centro == null ? null : MapToDto(centro);
    }

    public async Task<List<CentroDeCustoDto>> GetAllAsync(bool onlyActive = true)
    {
        var centros = await _context.CentrosDeCusto
            .Where(c => !onlyActive || c.Ativo)
            .ToListAsync();
        return centros.Select(MapToDto).ToList();
    }

    public async Task<CentroDeCustoDto?> UpdateAsync(int id, CreateCentroDeCustoDto dto)
    {
        var centro = await _context.CentrosDeCusto.FindAsync(id);
        if (centro == null) return null;

        centro.Nome = dto.Nome;
        centro.Descricao = dto.Descricao;
        centro.AtualizadoEm = DateTime.UtcNow;

        _context.CentrosDeCusto.Update(centro);
        await _context.SaveChangesAsync();

        return MapToDto(centro);
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var centro = await _context.CentrosDeCusto.FindAsync(id);
        if (centro == null) return false;

        centro.Ativo = false;
        centro.AtualizadoEm = DateTime.UtcNow;

        _context.CentrosDeCusto.Update(centro);
        await _context.SaveChangesAsync();
        return true;
    }

    private CentroDeCustoDto MapToDto(CentroDeCusto centro)
    {
        return new CentroDeCustoDto
        {
            Id = centro.Id,
            Nome = centro.Nome,
            Descricao = centro.Descricao,
            Ativo = centro.Ativo,
            CriadoEm = centro.CriadoEm,
            AtualizadoEm = centro.AtualizadoEm
        };
    }
}
