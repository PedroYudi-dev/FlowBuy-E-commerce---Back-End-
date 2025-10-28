
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.Interfaces.Services;
using api_ecommerce.Domain.Services;
using api_ecommerce.Infrastructure.Repositories;
using api_ecommerce.Infrastructure.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// Configurao de serviços:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Para o Swagger
builder.Services.AddSwaggerGen();           // Documentao Swagger

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Injeção de dependência dos repositórios:
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();
builder.Services.AddTransient<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddTransient<IProdutoRepository, ProdutoRepository>();
builder.Services.AddTransient<IEstoqueRepository, EstoqueRepository>();
builder.Services.AddTransient<IVendaRepository, VendaRepository>();

// Injeção do serviço de domínio:
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();

// Configuração do DbContext com MySQL:
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var app = builder.Build();

// Middlewares:
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Recomendado para produto
app.UseCors("AllowReactApp");
app.UseAuthorization();    // Habilita o uso de [Authorize], se necessário

app.MapControllers(); // Mapeia os endpoints das APIs

app.Run();
