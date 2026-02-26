using PlanoDePagamento.Data;
using PlanoDePagamento.DTOs;
using PlanoDePagamento.Models;
using PlanoDePagamento.Enums;
using Microsoft.EntityFrameworkCore;

namespace PlanoDePagamento.Services;

public class PlanoDePagamentoService
{
    private readonly AppDbContext _context;
    private readonly CobrancaService _cobrancaService;

    public PlanoDePagamentoService(AppDbContext context, CobrancaService cobrancaService)
    {
        _context = context;
        _cobrancaService = cobrancaService;
    }

    public async Task<PlanoDePagamentoDto?> CreateAsync(CreatePlanoDePagamentoDto dto)
    {
        // Validate responsavel exists
        var responsavel = await _context.ResponsaveisFinanceiros.FindAsync(dto.ResponsavelId);
        if (responsavel == null)
            throw new ArgumentException("Responsável financeiro not found");

        // Validate centro de custo exists
        var centro = await _context.CentrosDeCusto.FindAsync(dto.CentroDeCustoId);
        if (centro == null)
            throw new ArgumentException("Centro de custo not found");

        var plano = new Models.PlanoDePagamento
        {
            ResponsavelFinanceiroId = dto.ResponsavelId,
            CentroDeCustoId = dto.CentroDeCustoId,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };

        // Add cobrancas
        foreach (var cobrancaDto in dto.Cobrancas)
        {
            var cobranca = new Cobranca
            {
                Valor = cobrancaDto.Valor,
                DataVencimento = cobrancaDto.DataVencimento,
                MetodoPagamento = cobrancaDto.MetodoPagamento,
                Status = StatusCobranca.EMITIDA,
                CodigoPagamento = GenerateCodigoPagamento(cobrancaDto.MetodoPagamento),
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow
            };
            plano.Cobrancas.Add(cobranca);
            plano.ValorTotal += cobranca.Valor;
        }

        _context.PlanosDePagamento.Add(plano);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(plano.Id);
    }

    public async Task<PlanoDePagamentoDto?> GetByIdAsync(int id)
    {
        var plano = await _context.PlanosDePagamento
            .Include(p => p.ResponsavelFinanceiro)
            .Include(p => p.CentroDeCusto)
            .Include(p => p.Cobrancas)
            .FirstOrDefaultAsync(p => p.Id == id);

        return plano == null ? null : MapToDto((Models.PlanoDePagamento)plano!);
    }

    public async Task<decimal> GetTotalAsync(int id)
    {
        var plano = await _context.PlanosDePagamento.FindAsync(id);
        return plano?.ValorTotal ?? 0M;
    }

    public async Task<List<PlanoDePagamentoDto>> GetByResponsavelAsync(int responsavelId)
    {
        var planos = await _context.PlanosDePagamento
            .Where(p => p.ResponsavelFinanceiroId == responsavelId)
            .Include(p => p.ResponsavelFinanceiro)
            .Include(p => p.CentroDeCusto)
            .Include(p => p.Cobrancas)
            .ToListAsync();

        return planos.Cast<Models.PlanoDePagamento>().Select(MapToDto).ToList();
    }

    public async Task<List<CobrancaDto>> GetCobrancasByResponsavelAsync(int responsavelId)
    {
        var cobrancas = await _context.Cobrancas
            .Where(c => c.PlanoDePagamento!.ResponsavelFinanceiroId == responsavelId)
            .Include(c => c.PlanoDePagamento)
            .ToListAsync();

        return cobrancas.Select(c => _cobrancaService.MapCobrancaToDto(c)).ToList();
    }

    public async Task<int> GetQuantidadeCobrancasResponsavelAsync(int responsavelId)
    {
        return await _context.Cobrancas
            .Where(c => c.PlanoDePagamento!.ResponsavelFinanceiroId == responsavelId)
            .CountAsync();
    }

    private string GenerateCodigoPagamento(MetodoPagamento metodo)
    {
        if (metodo == MetodoPagamento.BOLETO)
        {
            // Simulated boleto linha digitável (47 digits format)
            return GenerateBoletoLinhaDigitavel();
        }
        else // PIX
        {
            // Simulated PIX code
            return GeneratePixCode();
        }
    }

    private string GenerateBoletoLinhaDigitavel()
    {
        // Format: NNNNN.NNNN NNNNN.NNNN NNNNN.NNNN N NNNNNNNNNNNNNN (47 digits)
        var random = new Random();
        var digits = "";
        for (int i = 0; i < 47; i++)
        {
            digits += random.Next(0, 10);
        }
        
        return $"{digits.Substring(0, 5)}.{digits.Substring(5, 4)} {digits.Substring(9, 5)}.{digits.Substring(14, 4)} {digits.Substring(18, 5)}.{digits.Substring(23, 4)} {digits.Substring(27, 1)} {digits.Substring(28, 14)}";
    }

    private string GeneratePixCode()
    {
        // Simulated PIX code (UUID-like format)
        return Guid.NewGuid().ToString();
    }

    private PlanoDePagamentoDto MapToDto(Models.PlanoDePagamento plano)
    {
        return new PlanoDePagamentoDto
        {
            Id = plano.Id,
            ResponsavelFinanceiroId = plano.ResponsavelFinanceiroId,
            CentroDeCustoId = plano.CentroDeCustoId,
            ValorTotal = plano.ValorTotal,
            Cobrancas = plano.Cobrancas?.Select(c => _cobrancaService.MapCobrancaToDto(c)).ToList() ?? new List<CobrancaDto>(),
            ResponsavelFinanceiro = plano.ResponsavelFinanceiro == null ? null : new ResponsavelFinanceiroDto
            {
                Id = plano.ResponsavelFinanceiro.Id,
                Nome = plano.ResponsavelFinanceiro.Nome,
                Identificador = plano.ResponsavelFinanceiro.Identificador,
                CriadoEm = plano.ResponsavelFinanceiro.CriadoEm,
                AtualizadoEm = plano.ResponsavelFinanceiro.AtualizadoEm
            },
            CentroDeCusto = plano.CentroDeCusto == null ? null : new CentroDeCustoDto
            {
                Id = plano.CentroDeCusto.Id,
                Nome = plano.CentroDeCusto.Nome,
                Descricao = plano.CentroDeCusto.Descricao,
                Ativo = plano.CentroDeCusto.Ativo,
                CriadoEm = plano.CentroDeCusto.CriadoEm,
                AtualizadoEm = plano.CentroDeCusto.AtualizadoEm
            },
            CriadoEm = plano.CriadoEm,
            AtualizadoEm = plano.AtualizadoEm
        };
    }
}
