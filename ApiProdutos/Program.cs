using ApiProdutos.Data;
using ApiProdutos.Entities.Validations;
using ApiProdutos.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("produtosApi"), new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddTransient<IValidator<Produto>, ProdutoValidator>();
builder.Services.AddTransient<IValidator<Cliente>, ClienteValidator>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
