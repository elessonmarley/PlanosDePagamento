using PlanoDePagamento.Data;
using PlanoDePagamento.DTOs;
using PlanoDePagamento.Models;
using PlanoDePagamento.Enums;
using Microsoft.EntityFrameworkCore;

namespace PlanoDePagamento.Services;

public class CobrancaService
{
    private readonly AppDbContext _context;

    public CobrancaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CobrancaDto?> GetByIdAsync(int id)
    {
        var cobranca = await _context.Cobrancas.Include(c => c.PlanoDePagamento).FirstOrDefaultAsync(c => c.Id == id);
        return cobranca == null ? null : MapCobrancaToDto(cobranca);
    }

    public async Task<List<CobrancaDto>> GetByPlanAsync(int planoId)
    {
        var cobrancas = await _context.Cobrancas
            .Where(c => c.PlanoDePagamentoId == planoId)
            .ToListAsync();

        return cobrancas.Select(MapCobrancaToDto).ToList();
    }

    public async Task<CobrancaDto?> RegistrarPagamentoAsync(int cobrancaId, RegistrarPagamentoDto dto)
    {
        var cobranca = await _context.Cobrancas
            .Include(c => c.Pagamentos)
            .FirstOrDefaultAsync(c => c.Id == cobrancaId);

        if (cobranca == null)
            throw new ArgumentException("Cobrança not found");

        // Não é permitido registrar pagamento em cobrança CANCELADA
        if (cobranca.Status == StatusCobranca.CANCELADA)
            throw new InvalidOperationException("Cannot register payment for CANCELADA charges");

        // Create payment record
        var pagamento = new Pagamento
        {
            CobrancaId = cobrancaId,
            Valor = dto.Valor,
            DataPagamento = dto.DataPagamento,
            CriadoEm = DateTime.UtcNow
        };

        cobranca.Pagamentos.Add(pagamento);

        // Update cobranca status to PAGA (considering total payment)
        var totalPago = cobranca.Pagamentos.Sum(p => p.Valor) + dto.Valor;
        if (totalPago >= cobranca.Valor)
        {
            cobranca.Status = StatusCobranca.PAGA;
        }

        cobranca.AtualizadoEm = DateTime.UtcNow;
        _context.Cobrancas.Update(cobranca);
        await _context.SaveChangesAsync();

        return MapCobrancaToDto(cobranca);
    }

    public async Task<CobrancaDto?> CancelarAsync(int cobrancaId)
    {
        var cobranca = await _context.Cobrancas.FindAsync(cobrancaId);
        if (cobranca == null)
            throw new ArgumentException("Cobrança not found");

        if (cobranca.Status == StatusCobranca.PAGA)
            throw new InvalidOperationException("Cannot cancel a PAID charge");

        cobranca.Status = StatusCobranca.CANCELADA;
        cobranca.AtualizadoEm = DateTime.UtcNow;

        _context.Cobrancas.Update(cobranca);
        await _context.SaveChangesAsync();

        return MapCobrancaToDto(cobranca);
    }

    public CobrancaDto MapCobrancaToDto(Cobranca cobranca)
    {
        return new CobrancaDto
        {
            Id = cobranca.Id,
            PlanoDePagamentoId = cobranca.PlanoDePagamentoId,
            Valor = cobranca.Valor,
            DataVencimento = cobranca.DataVencimento,
            MetodoPagamento = cobranca.MetodoPagamento,
            Status = cobranca.Status,
            CodigoPagamento = cobranca.CodigoPagamento,
            Vencida = cobranca.EstaVencida(),
            CriadoEm = cobranca.CriadoEm,
            AtualizadoEm = cobranca.AtualizadoEm
        };
    }
}
