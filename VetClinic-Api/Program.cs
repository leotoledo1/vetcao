using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;
using VetClinic.Repositories;
using VetClinic.Services;
using VetClinic.UseCases;

// Ponto de entrada da API: configura DI, CORS, banco SQLite e rotas.
/// ========================================
/// ARQUIVO: Program.cs
/// DESCRIÇÃO: Arquivo principal da aplicação
/// RESPONSABILIDADE: Configurar a API, conectar banco de dados e registrar dependências
/// 
/// O QUE ACONTECE AQUI:
/// 1. Cria a aplicação web
/// 2. Configura as portas e URLs
/// 3. Ativa CORS (permite que frontend acesse a API)
/// 4. Conecta ao banco de dados SQLite
/// 5. Registra Services e Repositories (injeção de dependência)
/// 6. Inicializa dados de teste (veterinários)
/// ========================================

var builder = WebApplication.CreateBuilder(args);

// PASSO 1: Configurar a porta HTTP da API
// Se estiver em modo Desenvolvimento, usar porta 5000
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://localhost:5000");
}

// PASSO 2: Registrar Controllers (responsáveis pelas rotas HTTP)
builder.Services.AddControllers();

// PASSO 3: Ativar Swagger/OpenAPI (documentação automática da API)
builder.Services.AddOpenApi();

// PASSO 4: Configurar CORS (permite que o frontend acesse a API)
// Sem CORS, o frontend em localhost:5173 NÃO conseguiria fazer requisições para a API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Aceita requisições de:
        // - http://localhost:5173 (Vite em desenvolvimento)
        // - http://127.0.0.1:5173 (Vite em desenvolvimento - IP local)
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
              .AllowAnyHeader()      // Aceita qualquer header nas requisições
              .AllowAnyMethod();      // Aceita GET, POST, PUT, DELETE, etc
    });
});

// PASSO 5: Configurar Banco de Dados SQLite
// EntityFramework Core vai gerenciar as tabelas automaticamente
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite("Data Source=vetclinic.db")
);

// PASSO 6: Registrar REPOSITORIES (padrão Repository)
// O Repository é responsável por acessar o banco de dados
// Usamos Interface para permitir trocar a implementação facilmente
// AddScoped = cria uma nova instância por requisição HTTP
builder.Services.AddScoped<IDonoRepository, DonoRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();

// PASSO 7: Registrar SERVICES (padrão Service)
// O Service contém a lógica de negócio (regras, validações)
// Exemplo: DonoService valida CPF, verifica duplicatas, etc
builder.Services.AddScoped<IDonoService, DonoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

// PASSO 8: Registrar USE CASES (padrão Use Case)
// Um Use Case representa uma ação específica do sistema
// Exemplo: "Agendar Consulta" é um use case que valida dados e agendar
builder.Services.AddScoped<CadastrarDonoUseCase>();
builder.Services.AddScoped<CadastrarPetUseCase>();
builder.Services.AddScoped<AgendarConsultaUseCase>();
builder.Services.AddScoped<CancelarConsultaUseCase>();
builder.Services.AddScoped<ExcluirPetUseCase>();

var app = builder.Build();

// PASSO 9: Inicializar o Banco de Dados
// Este código roda apenas na primeira vez que a aplicação inicia
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    
    // EnsureCreated() cria as tabelas se não existirem
    db.Database.EnsureCreated();

    // Se não houver veterinários cadastrados, adicionar os padrões
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

// PASSO 10: Configurar o Pipeline de Requisições HTTP
if (app.Environment.IsDevelopment())
{
    // Ativar documentação Swagger (acessível em http://localhost:5000/openapi/v1.json)
    app.MapOpenApi();
}

// Ativar CORS - DEVE estar ANTES de MapControllers()
app.UseCors();

// Ativar autorização (autenticação)
app.UseAuthorization();

// Mapear todos os Controllers e suas rotas HTTP
app.MapControllers();

// PASSO 11: Iniciar a aplicação
// A API agora está ouvindo em http://localhost:5000
app.Run();
