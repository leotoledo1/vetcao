using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;
using VetClinic.Repositories;
using VetClinic.Services;
using VetClinic.UseCases;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://localhost:5000");
}

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite("Data Source=vetclinic.db")
);

builder.Services.AddScoped<IDonoRepository, DonoRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();

builder.Services.AddScoped<IDonoService, DonoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

builder.Services.AddScoped<CadastrarDonoUseCase>();
builder.Services.AddScoped<CadastrarPetUseCase>();
builder.Services.AddScoped<AgendarConsultaUseCase>();
builder.Services.AddScoped<CancelarConsultaUseCase>();
builder.Services.AddScoped<ExcluirPetUseCase>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    db.Database.EnsureCreated();

    if (!db.Veterinarios.Any())
    {
        db.Veterinarios.AddRange(
            new Veterinario
            {
                Nome = "Ana Souza",
                Crmv = "CRMV-SP-1001",
                Especialidade = "Clinica Geral",
                CreatedAt = DateTime.Now
            },
            new Veterinario
            {
                Nome = "Bruno Lima",
                Crmv = "CRMV-SP-1002",
                Especialidade = "Dermatologia",
                CreatedAt = DateTime.Now
            },
            new Veterinario
            {
                Nome = "Carla Mendes",
                Crmv = "CRMV-SP-1003",
                Especialidade = "Ortopedia",
                CreatedAt = DateTime.Now
            }
        );

        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
