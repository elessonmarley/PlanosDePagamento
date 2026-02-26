using PlanoDePagamento.Data;
using PlanoDePagamento.DTOs;
using PlanoDePagamento.Models;
using Microsoft.EntityFrameworkCore;

namespace PlanoDePagamento.Services;

public class ResponsavelFinanceiroService
{
    private readonly AppDbContext _context;

    public ResponsavelFinanceiroService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponsavelFinanceiroDto?> CreateAsync(CreateResponsavelFinanceiroDto dto)
    {
        var responsavel = new ResponsavelFinanceiro
        {
            Nome = dto.Nome,
            Identificador = dto.Identificador,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };

        _context.ResponsaveisFinanceiros.Add(responsavel);
        await _context.SaveChangesAsync();

        return MapToDto(responsavel);
    }

    public async Task<ResponsavelFinanceiroDto?> GetByIdAsync(int id)
    {
        var responsavel = await _context.ResponsaveisFinanceiros.FindAsync(id);
        return responsavel == null ? null : MapToDto(responsavel);
    }

    public async Task<List<ResponsavelFinanceiroDto>> GetAllAsync()
    {
        var responsaveis = await _context.ResponsaveisFinanceiros.ToListAsync();
        return responsaveis.Select(MapToDto).ToList();
    }

    public async Task<ResponsavelFinanceiroDto?> UpdateAsync(int id, CreateResponsavelFinanceiroDto dto)
    {
        var responsavel = await _context.ResponsaveisFinanceiros.FindAsync(id);
        if (responsavel == null) return null;

        responsavel.Nome = dto.Nome;
        responsavel.Identificador = dto.Identificador;
        responsavel.AtualizadoEm = DateTime.UtcNow;

        _context.ResponsaveisFinanceiros.Update(responsavel);
        await _context.SaveChangesAsync();

        return MapToDto(responsavel);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var responsavel = await _context.ResponsaveisFinanceiros.FindAsync(id);
        if (responsavel == null) return false;

        _context.ResponsaveisFinanceiros.Remove(responsavel);
        await _context.SaveChangesAsync();
        return true;
    }

    private ResponsavelFinanceiroDto MapToDto(ResponsavelFinanceiro responsavel)
    {
        return new ResponsavelFinanceiroDto
        {
            Id = responsavel.Id,
            Nome = responsavel.Nome,
            Identificador = responsavel.Identificador,
            CriadoEm = responsavel.CriadoEm,
            AtualizadoEm = responsavel.AtualizadoEm
        };
    }
}
