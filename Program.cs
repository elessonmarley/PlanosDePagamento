using Microsoft.EntityFrameworkCore;
using PlanoDePagamento.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddNpgsql<AppDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")
);

// Add services
builder.Services.AddScoped<PlanoDePagamento.Services.ResponsavelFinanceiroService>();
builder.Services.AddScoped<PlanoDePagamento.Services.CentroDeCustoService>();
builder.Services.AddScoped<PlanoDePagamento.Services.PlanoDePagamentoService>();
builder.Services.AddScoped<PlanoDePagamento.Services.CobrancaService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
