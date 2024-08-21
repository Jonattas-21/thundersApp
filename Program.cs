using Infrastructure.Databases;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repositories;
using Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOutputCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Thunders api",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Jonattas Pedroso",
            Email = "jonattas-21@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/jonattas-pedroso-a251a82b/")
        }
    });
});

builder.Services.AddScoped<IOriginService, OriginService>();
builder.Services.AddScoped<ITaskForceService, TaskForceService>();
builder.Services.AddScoped<IRepository<TaskForce>, Repository<TaskForce>>();
builder.Services.AddScoped<IRepository<Origin>, Repository<Origin>>();

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

app.UseOutputCache();

//app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
