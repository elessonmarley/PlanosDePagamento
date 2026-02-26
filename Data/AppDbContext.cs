using Microsoft.EntityFrameworkCore;
using PlanoDePagamento.Models;

namespace PlanoDePagamento.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ResponsavelFinanceiro> ResponsaveisFinanceiros { get; set; }
    public DbSet<CentroDeCusto> CentrosDeCusto { get; set; }
    public DbSet<Models.PlanoDePagamento> PlanosDePagamento { get; set; }
    public DbSet<Cobranca> Cobrancas { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ResponsavelFinanceiro
        modelBuilder.Entity<ResponsavelFinanceiro>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Identificador).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Identificador).IsUnique();
        });

        // CentroDeCusto
        modelBuilder.Entity<CentroDeCusto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
        });

        // PlanoDePagamento
        modelBuilder.Entity<Models.PlanoDePagamento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ValorTotal).HasColumnType("numeric(18,2)");
            
            entity.HasOne(e => e.ResponsavelFinanceiro)
                .WithMany(r => r.PlanosDePagamento)
                .HasForeignKey(e => e.ResponsavelFinanceiroId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CentroDeCusto)
                .WithMany(c => c.PlanosDePagamento)
                .HasForeignKey(e => e.CentroDeCustoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Cobranca
        modelBuilder.Entity<Cobranca>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Valor).HasColumnType("numeric(18,2)");
            entity.Property(e => e.CodigoPagamento).HasMaxLength(100);
            entity.Property(e => e.MetodoPagamento).HasConversion<int>();
            entity.Property(e => e.Status).HasConversion<int>();

            entity.HasOne(e => e.PlanoDePagamento)
                .WithMany(p => p.Cobrancas)
                .HasForeignKey(e => e.PlanoDePagamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.CodigoPagamento).IsUnique();
        });

        // Pagamento
        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Valor).HasColumnType("numeric(18,2)");

            entity.HasOne(e => e.Cobranca)
                .WithMany(c => c.Pagamentos)
                .HasForeignKey(e => e.CobrancaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed default cost centers
        modelBuilder.Entity<CentroDeCusto>().HasData(
            new CentroDeCusto { Id = 1, Nome = "MATRÍCULA", Descricao = "Taxa de matrícula", CriadoEm = DateTime.UtcNow, AtualizadoEm = DateTime.UtcNow },
            new CentroDeCusto { Id = 2, Nome = "MENSALIDADE", Descricao = "Mensalidade escolar/acadêmica", CriadoEm = DateTime.UtcNow, AtualizadoEm = DateTime.UtcNow },
            new CentroDeCusto { Id = 3, Nome = "MATERIAL", Descricao = "Material didático/escolar", CriadoEm = DateTime.UtcNow, AtualizadoEm = DateTime.UtcNow }
        );
    }
}
