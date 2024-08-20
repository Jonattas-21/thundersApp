using Infrastructure.Databases;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AwesomeDevEvents.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Jonattas Pedroso",
            Email = "jonattas-21@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/jonattas-pedroso-a251a82b/")
        }
    });
});


builder.Services.AddScoped<IWineService, WineServices>();
builder.Services.AddScoped<IGrapeService, GrapeServices>();
builder.Services.AddScoped<IRepository<Wine>, Repository<Wine>>();
builder.Services.AddScoped<IRepository<Grape>, Repository<Grape>>();
builder.Services.AddScoped<IRepository<Analysis>, Repository<Analysis>>();


builder.Logging.ClearProviders().AddConsole();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Infrastructure")).UseLazyLoadingProxies());


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
