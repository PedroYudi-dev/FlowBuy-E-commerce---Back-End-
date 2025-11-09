
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.Interfaces.Services;
using api_ecommerce.Domain.Services;
using api_ecommerce.Infrastructure.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurao de servi?os:
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

builder.Services.AddControllers()
    .AddJsonOptions(jsonResp =>
    {
        jsonResp.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        jsonResp.JsonSerializerOptions.WriteIndented = true;
    });

// Inje??o de depend?ncia dos reposit?rios:
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();
builder.Services.AddTransient<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddTransient<IProdutoRepository, ProdutoRepository>();
builder.Services.AddTransient<IEstoqueRepository, EstoqueRepository>();
builder.Services.AddTransient<IVendaRepository, VendaRepository>();

// Inje??o do servi?o de dom?nio:
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
builder.Services.AddScoped<ProdutoService>();

// Configura??o do DbContext com MySQL:
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
app.UseAuthorization();    // Habilita o uso de [Authorize], se necess?rio

app.MapControllers(); // Mapeia os endpoints das APIs

app.Run();