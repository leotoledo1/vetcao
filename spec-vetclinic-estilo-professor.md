# SPEC TÉCNICA — VetClinic
## UNIMAR · Trabalho Final · Professor William Castro
## Baseada no padrão do projeto UniBet

> **Stack:** C# .NET 10 + React + SQLite (Entity Framework Core)
> **Padrão:** Mesmo estilo do UniBet — Entities, ValueObjects, UseCases, Services, Repositories, Controllers, DTOs
> **Banco:** SQLite com EF Core (mesmo padrão do Context.cs do UniBet, mas SQLite)

---

## SUMÁRIO

1. Padrões do Professor (extraídos do UniBet)
2. Estrutura do Projeto
3. Entities
4. ValueObjects
5. DTOs e Requests
6. Interfaces
7. UseCases
8. Services
9. Repositories
10. Controllers
11. Data/Context
12. Program.cs
13. Frontend React
14. Regras de Negócio
15. Instruções para o Codex

---

## 1. PADRÕES DO PROFESSOR (extraídos do UniBet)

O Codex deve seguir EXATAMENTE estes padrões observados no projeto UniBet:

```
PADRÃO DE ENTITY:
  - Herda de EntityBase (que tem Id, CreatedAt, RemovedAt)
  - Propriedades públicas com get/set
  - Métodos de negócio dentro da própria entidade
  - Exceções com throw new Exception("mensagem")

PADRÃO DE VALUE OBJECT:
  - Classe simples com propriedade Value { get; private set; }
  - Construtor valida e atribui
  - Lança Exception se inválido

PADRÃO DE USE CASE:
  - Classe com Run() (não ExecutarAsync — usar Run() igual ao UniBet)
  - Recebe repositories via construtor
  - Busca entidade → valida → chama método da entidade → chama repository
  - try/catch relançando Exception

PADRÃO DE CONTROLLER:
  - [ApiController] [Route("[controller]")]
  - Injeta UseCase ou Service via construtor
  - try/catch retornando BadRequest(ex.Message)
  - Métodos nomeados como ações: Agendar, Listar, Cancelar

PADRÃO DE REPOSITORY:
  - Recebe Context via construtor
  - Métodos: GetAll(), FindById(Guid), Create(), Update(), Delete()
  - Usa _database.DbSet diretamente

PADRÃO DE SERVICE:
  - Implementa interface IXService
  - Recebe IXRepository via construtor
  - try/catch em todos os métodos

PADRÃO DE INTERFACE:
  - IRepositories/ e IServices/ em pastas separadas
  - Métodos síncronos (GetAll, FindById, Create, Update, Delete)

PADRÃO DE DTO:
  - Classes simples com propriedades públicas
  - DTOs de criação em DTOs/
  - Requests de use cases em DTOs/Requests/

BANCO:
  - EF Core com DbContext (Context.cs)
  - SQLite (trocar MySQL do UniBet por SQLite)
  - Migrations ou EnsureCreated no Program.cs

VERSÃO: .NET 10 (igual ao UniBet)
```

---

## 2. ESTRUTURA DO PROJETO

```
VetClinic/
│
├── VetClinic.sln
│
├── VetClinic/                              ← projeto backend (igual UniBet)
│   ├── VetClinic.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   │
│   ├── Controllers/                        ← Bounded Context separados por controller
│   │   ├── DonoController.cs               ← BC1: Cadastro
│   │   ├── PetController.cs                ← BC1: Cadastro
│   │   └── ConsultaController.cs           ← BC2: Consultas
│   │
│   ├── Data/
│   │   └── Context.cs                      ← EF Core DbContext (igual UniBet)
│   │
│   ├── DTOs/
│   │   ├── CreateDonoDTO.cs
│   │   ├── CreatePetDTO.cs
│   │   ├── CreateConsultaDTO.cs
│   │   └── Requests/
│   │       ├── AgendarConsultaRequest.cs
│   │       └── CancelarConsultaRequest.cs
│   │
│   ├── Entities/
│   │   ├── EntityBase.cs                   ← IGUAL ao UniBet
│   │   ├── Dono.cs
│   │   ├── Pet.cs
│   │   ├── Veterinario.cs
│   │   └── Consulta.cs
│   │
│   ├── Interfaces/
│   │   ├── IRepositories/
│   │   │   ├── IDonoRepository.cs
│   │   │   ├── IPetRepository.cs
│   │   │   ├── IVeterinarioRepository.cs
│   │   │   └── IConsultaRepository.cs
│   │   └── IServices/
│   │       ├── IDonoService.cs
│   │       └── IConsultaService.cs
│   │
│   ├── Repositories/
│   │   ├── DonoRepository.cs
│   │   ├── PetRepository.cs
│   │   ├── VeterinarioRepository.cs
│   │   └── ConsultaRepository.cs
│   │
│   ├── Services/
│   │   ├── DonoService.cs
│   │   └── ConsultaService.cs
│   │
│   ├── UseCases/
│   │   ├── CadastrarDonoUseCase.cs
│   │   ├── CadastrarPetUseCase.cs
│   │   ├── AgendarConsultaUseCase.cs
│   │   ├── CancelarConsultaUseCase.cs
│   │   └── ExcluirPetUseCase.cs
│   │
│   └── ValueObjects/
│       └── Cpf.cs
│
└── frontend/
    └── vetclinic-web/                      ← React com Vite
```

---

## 3. ENTITIES

### EntityBase.cs — IGUAL ao UniBet

```csharp
namespace VetClinic.Entities
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime? RemovedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public EntityBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
```

### Dono.cs

```csharp
using VetClinic.ValueObjects;

namespace VetClinic.Entities
{
    public class Dono : EntityBase
    {
        public string Nome { get; set; }
        public Cpf Cpf { get; set; }           // ValueObject — RN01
        public string Telefone { get; set; }
        public string Email { get; set; }

        // Lista de pets do dono
        private readonly List<Pet> _pets = new List<Pet>();
        public IReadOnlyCollection<Pet> Pets => _pets.AsReadOnly();

        public Dono() { }

        // RN02: método de negócio dentro da entidade
        public void AdicionarPet(Pet pet)
        {
            if (pet == null)
            {
                throw new Exception("Pet inválido");
            }

            _pets.Add(pet);
        }
    }
}
```

### Pet.cs

```csharp
namespace VetClinic.Entities
{
    public class Pet : EntityBase
    {
        public string Nome { get; set; }
        public string Especie { get; set; }
        public string Raca { get; set; }
        public DateTime DataNascimento { get; set; }
        public Guid DonoId { get; set; }
        public Dono Dono { get; set; }

        public Pet() { }

        // Validação dentro da entidade
        public void Validar()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new Exception("Nome do pet é obrigatório");
            }

            if (DataNascimento > DateTime.Now)
            {
                throw new Exception("Data de nascimento não pode ser futura");
            }
        }
    }
}
```

### Veterinario.cs

```csharp
namespace VetClinic.Entities
{
    public class Veterinario : EntityBase
    {
        public string Nome { get; set; }
        public string Crmv { get; set; }
        public string Especialidade { get; set; }

        public Veterinario() { }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new Exception("Nome do veterinário é obrigatório");
            }

            if (string.IsNullOrEmpty(Crmv))
            {
                throw new Exception("CRMV é obrigatório");
            }
        }
    }
}
```

### Consulta.cs

```csharp
namespace VetClinic.Entities
{
    public class Consulta : EntityBase
    {
        public Guid PetId { get; set; }
        public Pet Pet { get; set; }
        public Guid VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }
        public DateTime DataHora { get; set; }
        public string Motivo { get; set; }
        public string Status { get; set; }      // Agendada | Realizada | Cancelada

        public Consulta() { }

        // RN05 — data não pode ser passada
        public void Validar()
        {
            if (DataHora < DateTime.Now)
            {
                throw new Exception("A data da consulta não pode ser no passado");
            }

            if (string.IsNullOrEmpty(Motivo))
            {
                throw new Exception("Motivo da consulta é obrigatório");
            }
        }

        // RN06 — só cancela se agendada
        public void Cancelar()
        {
            if (Status != "Agendada")
            {
                throw new Exception("Apenas consultas com status Agendada podem ser canceladas");
            }

            this.Status = "Cancelada";
        }
    }
}
```

---

## 4. VALUE OBJECTS

### Cpf.cs — mesmo padrão do Email.cs do UniBet

```csharp
namespace VetClinic.ValueObjects
{
    public class Cpf
    {
        public string Value { get; private set; }

        public Cpf(string value)
        {
            // RN01: validação de formato
            var numeros = new string(value.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(value) || numeros.Length != 11)
            {
                throw new Exception("CPF com formato inválido");
            }

            this.Value = numeros;   // armazena só os dígitos
        }
    }
}
```

---

## 5. DTOs E REQUESTS

### CreateDonoDTO.cs

```csharp
namespace VetClinic.DTOs
{
    public class CreateDonoDTO
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}
```

### CreatePetDTO.cs

```csharp
namespace VetClinic.DTOs
{
    public class CreatePetDTO
    {
        public string Nome { get; set; }
        public string Especie { get; set; }
        public string Raca { get; set; }
        public DateTime DataNascimento { get; set; }
        public Guid DonoId { get; set; }
    }
}
```

### CreateConsultaDTO.cs

```csharp
namespace VetClinic.DTOs
{
    public class CreateConsultaDTO
    {
        public Guid PetId { get; set; }
        public Guid VeterinarioId { get; set; }
        public DateTime DataHora { get; set; }
        public string Motivo { get; set; }
    }
}
```

### Requests/AgendarConsultaRequest.cs

```csharp
namespace VetClinic.DTOs.Requests
{
    public class AgendarConsultaRequest
    {
        public Guid PetId { get; set; }
        public Guid VeterinarioId { get; set; }
        public DateTime DataHora { get; set; }
        public string Motivo { get; set; }
    }
}
```

---

## 6. INTERFACES

### IRepositories/IDonoRepository.cs — mesmo padrão do IUserRepository do UniBet

```csharp
using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    public interface IDonoRepository
    {
        public List<Dono> GetAll();
        public Dono FindById(Guid id);
        public bool CpfJaExiste(string cpf);   // RN01
        public void Create(Dono dono);
        public void Update(Dono dono);
        public void Delete(Dono dono);
    }
}
```

### IRepositories/IPetRepository.cs

```csharp
using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    public interface IPetRepository
    {
        public List<Pet> GetAll();
        public Pet FindById(Guid id);
        public void Create(Pet pet);
        public void Update(Pet pet);
        public void Delete(Pet pet);
        public bool TemConsultaFutura(Guid petId);   // RN04
    }
}
```

### IRepositories/IConsultaRepository.cs

```csharp
using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    public interface IConsultaRepository
    {
        public List<Consulta> GetAll();
        public Consulta FindById(Guid id);
        public void Create(Consulta consulta);
        public void Update(Consulta consulta);
        public bool TemConflitoDeHorario(Guid veterinarioId, DateTime dataHora);  // RN03
    }
}
```

### IRepositories/IVeterinarioRepository.cs

```csharp
using VetClinic.Entities;

namespace VetClinic.Interfaces.IRepositories
{
    public interface IVeterinarioRepository
    {
        public List<Veterinario> GetAll();
        public Veterinario FindById(Guid id);
        public void Create(Veterinario veterinario);
    }
}
```

### IServices/IDonoService.cs — mesmo padrão do IUserService do UniBet

```csharp
using VetClinic.DTOs;
using VetClinic.Entities;

namespace VetClinic.Interfaces.IServices
{
    public interface IDonoService
    {
        public List<Dono> ListarDonos();
        public Dono GetById(Guid id);
        public void CriarDono(CreateDonoDTO dto);
        public void DeletarDono(Guid id);
    }
}
```

### IServices/IConsultaService.cs

```csharp
using VetClinic.DTOs;
using VetClinic.Entities;

namespace VetClinic.Interfaces.IServices
{
    public interface IConsultaService
    {
        public List<Consulta> ListarConsultas();
        public Consulta GetById(Guid id);
    }
}
```

---

## 7. USE CASES — mesmo padrão do PlaceGameUseCase do UniBet

### CadastrarDonoUseCase.cs

```csharp
using VetClinic.DTOs;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.ValueObjects;

namespace VetClinic.UseCases
{
    public class CadastrarDonoUseCase
    {
        private readonly IDonoRepository _donoRepository;

        public CadastrarDonoUseCase(IDonoRepository donoRepository)
        {
            _donoRepository = donoRepository;
        }

        public void Run(CreateDonoDTO dto)
        {
            try
            {
                // RN01: CPF não pode ser duplicado
                bool cpfExiste = _donoRepository.CpfJaExiste(
                    new string(dto.Cpf.Where(char.IsDigit).ToArray())
                );

                if (cpfExiste)
                {
                    throw new Exception("CPF já cadastrado no sistema");
                }

                Dono dono = new Dono();
                dono.Nome = dto.Nome;
                dono.Cpf = new Cpf(dto.Cpf);   // ValueObject valida o formato
                dono.Telefone = dto.Telefone;
                dono.Email = dto.Email;
                dono.CreatedAt = DateTime.Now;

                _donoRepository.Create(dono);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
```

### CadastrarPetUseCase.cs

```csharp
using VetClinic.DTOs;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    public class CadastrarPetUseCase
    {
        private readonly IPetRepository _petRepository;
        private readonly IDonoRepository _donoRepository;

        public CadastrarPetUseCase(IPetRepository petRepository, IDonoRepository donoRepository)
        {
            _petRepository = petRepository;
            _donoRepository = donoRepository;
        }

        public void Run(CreatePetDTO dto)
        {
            try
            {
                // RN02: dono deve existir
                Dono dono = _donoRepository.FindById(dto.DonoId);
                if (dono == null)
                {
                    throw new Exception("Dono não encontrado");
                }

                Pet pet = new Pet();
                pet.Nome = dto.Nome;
                pet.Especie = dto.Especie;
                pet.Raca = dto.Raca;
                pet.DataNascimento = dto.DataNascimento;
                pet.DonoId = dto.DonoId;
                pet.CreatedAt = DateTime.Now;

                // Validações dentro da entidade
                pet.Validar();

                _petRepository.Create(pet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
```

### AgendarConsultaUseCase.cs

```csharp
using VetClinic.DTOs.Requests;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    public class AgendarConsultaUseCase
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IPetRepository _petRepository;
        private readonly IVeterinarioRepository _veterinarioRepository;

        public AgendarConsultaUseCase(
            IConsultaRepository consultaRepository,
            IPetRepository petRepository,
            IVeterinarioRepository veterinarioRepository)
        {
            _consultaRepository = consultaRepository;
            _petRepository = petRepository;
            _veterinarioRepository = veterinarioRepository;
        }

        public void Run(AgendarConsultaRequest request)
        {
            try
            {
                // Verificar se pet existe
                Pet pet = _petRepository.FindById(request.PetId);
                if (pet == null)
                {
                    throw new Exception("Pet não encontrado");
                }

                // Verificar se veterinário existe
                Veterinario vet = _veterinarioRepository.FindById(request.VeterinarioId);
                if (vet == null)
                {
                    throw new Exception("Veterinário não encontrado");
                }

                // RN03: verificar conflito de horário
                bool temConflito = _consultaRepository.TemConflitoDeHorario(
                    request.VeterinarioId, request.DataHora
                );

                if (temConflito)
                {
                    throw new Exception("Veterinário já possui consulta neste horário");
                }

                Consulta consulta = new Consulta();
                consulta.PetId = request.PetId;
                consulta.VeterinarioId = request.VeterinarioId;
                consulta.DataHora = request.DataHora;
                consulta.Motivo = request.Motivo;
                consulta.Status = "Agendada";
                consulta.CreatedAt = DateTime.Now;

                // RN05: valida data dentro da entidade
                consulta.Validar();

                _consultaRepository.Create(consulta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
```

### CancelarConsultaUseCase.cs

```csharp
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    public class CancelarConsultaUseCase
    {
        private readonly IConsultaRepository _consultaRepository;

        public CancelarConsultaUseCase(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        public void Run(Guid consultaId)
        {
            try
            {
                var consulta = _consultaRepository.FindById(consultaId);
                if (consulta == null)
                {
                    throw new Exception("Consulta não encontrada");
                }

                // RN06: regra de cancelamento dentro da entidade
                consulta.Cancelar();

                _consultaRepository.Update(consulta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
```

### ExcluirPetUseCase.cs

```csharp
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.UseCases
{
    public class ExcluirPetUseCase
    {
        private readonly IPetRepository _petRepository;
        private readonly IConsultaRepository _consultaRepository;

        public ExcluirPetUseCase(IPetRepository petRepository, IConsultaRepository consultaRepository)
        {
            _petRepository = petRepository;
            _consultaRepository = consultaRepository;
        }

        public void Run(Guid petId)
        {
            try
            {
                var pet = _petRepository.FindById(petId);
                if (pet == null)
                {
                    throw new Exception("Pet não encontrado");
                }

                // RN04: não excluir pet com consulta futura
                bool temConsultaFutura = _consultaRepository.TemConflitoDeHorario == null
                    ? false
                    : _petRepository.TemConsultaFutura(petId);

                if (temConsultaFutura)
                {
                    throw new Exception("Não é possível excluir pet com consultas futuras agendadas");
                }

                pet.RemovedAt = DateTime.Now;
                _petRepository.Update(pet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
```

---

## 8. SERVICES — mesmo padrão do UserService do UniBet

### DonoService.cs

```csharp
using VetClinic.DTOs;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;

namespace VetClinic.Services
{
    public class DonoService : IDonoService
    {
        private readonly IDonoRepository _repository;

        public DonoService(IDonoRepository repository)
        {
            this._repository = repository;
        }

        public void CriarDono(CreateDonoDTO dto)
        {
            try
            {
                // delegado para o UseCase — Service só orquestra
                throw new NotImplementedException("Usar CadastrarDonoUseCase");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeletarDono(Guid id)
        {
            try
            {
                Dono dono = _repository.FindById(id);
                if (dono == null)
                {
                    throw new Exception("Dono não encontrado");
                }

                dono.RemovedAt = DateTime.Now;
                _repository.Update(dono);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Dono GetById(Guid id)
        {
            try
            {
                Dono dono = _repository.FindById(id);
                if (dono == null)
                {
                    return null;
                }
                return dono;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Dono> ListarDonos()
        {
            try
            {
                List<Dono> list = _repository.GetAll();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
```

### ConsultaService.cs

```csharp
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;

namespace VetClinic.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repository;

        public ConsultaService(IConsultaRepository repository)
        {
            this._repository = repository;
        }

        public Consulta GetById(Guid id)
        {
            try
            {
                return _repository.FindById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Consulta> ListarConsultas()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
```

---

## 9. REPOSITORIES — mesmo padrão do UserRepository do UniBet

### DonoRepository.cs

```csharp
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
    public class DonoRepository : IDonoRepository
    {
        public readonly Context _database;

        public DonoRepository(Context context)
        {
            _database = context;
        }

        public void Create(Dono dono)
        {
            _database.Donos.Add(dono);
            _database.SaveChanges();
        }

        public void Delete(Dono dono)
        {
            _database.Donos.Remove(dono);
            _database.SaveChanges();
        }

        public List<Dono> GetAll()
        {
            return _database.Donos
                .Where(d => d.RemovedAt == null)
                .ToList();
        }

        public Dono FindById(Guid id)
        {
            return _database.Donos
                .Where(d => d.Id == id && d.RemovedAt == null)
                .FirstOrDefault();
        }

        public bool CpfJaExiste(string cpf)
        {
            return _database.Donos
                .Any(d => d.Cpf.Value == cpf && d.RemovedAt == null);
        }

        public void Update(Dono dono)
        {
            _database.Donos.Update(dono);
            _database.SaveChanges();
        }
    }
}
```

### PetRepository.cs

```csharp
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
    public class PetRepository : IPetRepository
    {
        public readonly Context _database;

        public PetRepository(Context context)
        {
            _database = context;
        }

        public void Create(Pet pet)
        {
            _database.Pets.Add(pet);
            _database.SaveChanges();
        }

        public void Delete(Pet pet)
        {
            _database.Pets.Remove(pet);
            _database.SaveChanges();
        }

        public List<Pet> GetAll()
        {
            return _database.Pets
                .Where(p => p.RemovedAt == null)
                .ToList();
        }

        public Pet FindById(Guid id)
        {
            return _database.Pets
                .Where(p => p.Id == id && p.RemovedAt == null)
                .FirstOrDefault();
        }

        // RN04: verificar se pet tem consulta futura
        public bool TemConsultaFutura(Guid petId)
        {
            return _database.Consultas
                .Any(c => c.PetId == petId
                       && c.Status == "Agendada"
                       && c.DataHora > DateTime.Now);
        }

        public void Update(Pet pet)
        {
            _database.Pets.Update(pet);
            _database.SaveChanges();
        }
    }
}
```

### ConsultaRepository.cs

```csharp
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        public readonly Context _database;

        public ConsultaRepository(Context context)
        {
            _database = context;
        }

        public void Create(Consulta consulta)
        {
            _database.Consultas.Add(consulta);
            _database.SaveChanges();
        }

        public List<Consulta> GetAll()
        {
            return _database.Consultas.ToList();
        }

        public Consulta FindById(Guid id)
        {
            return _database.Consultas
                .Where(c => c.Id == id)
                .FirstOrDefault();
        }

        // RN03: conflito de horário — janela de 30 minutos
        public bool TemConflitoDeHorario(Guid veterinarioId, DateTime dataHora)
        {
            return _database.Consultas
                .Any(c => c.VeterinarioId == veterinarioId
                       && c.Status == "Agendada"
                       && Math.Abs((c.DataHora - dataHora).TotalMinutes) < 30);
        }

        public void Update(Consulta consulta)
        {
            _database.Consultas.Update(consulta);
            _database.SaveChanges();
        }
    }
}
```

### VeterinarioRepository.cs

```csharp
using VetClinic.Data;
using VetClinic.Entities;
using VetClinic.Interfaces.IRepositories;

namespace VetClinic.Repositories
{
    public class VeterinarioRepository : IVeterinarioRepository
    {
        public readonly Context _database;

        public VeterinarioRepository(Context context)
        {
            _database = context;
        }

        public void Create(Veterinario veterinario)
        {
            _database.Veterinarios.Add(veterinario);
            _database.SaveChanges();
        }

        public List<Veterinario> GetAll()
        {
            return _database.Veterinarios.ToList();
        }

        public Veterinario FindById(Guid id)
        {
            return _database.Veterinarios
                .Where(v => v.Id == id)
                .FirstOrDefault();
        }
    }
}
```

---

## 10. CONTROLLERS — mesmo padrão do GameController e UserController do UniBet

### DonoController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using VetClinic.DTOs;
using VetClinic.Interfaces.IServices;
using VetClinic.UseCases;

namespace VetClinic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DonoController : ControllerBase
    {
        private readonly IDonoService _service;
        private readonly CadastrarDonoUseCase _cadastrarDonoUseCase;

        public DonoController(IDonoService service, CadastrarDonoUseCase cadastrarDonoUseCase)
        {
            _service = service;
            _cadastrarDonoUseCase = cadastrarDonoUseCase;
        }

        [HttpGet("ListarDonos")]
        public IActionResult ListarDonos()
        {
            var list = _service.ListarDonos();
            return Ok(list);
        }

        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] Guid id)
        {
            try
            {
                var dono = _service.GetById(id);
                if (dono == null)
                {
                    return NotFound();
                }
                return Ok(dono);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CadastrarDono")]
        public IActionResult CadastrarDono([FromBody] CreateDonoDTO dto)
        {
            try
            {
                _cadastrarDonoUseCase.Run(dto);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
```

### PetController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using VetClinic.DTOs;
using VetClinic.Interfaces.IRepositories;
using VetClinic.UseCases;

namespace VetClinic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly CadastrarPetUseCase _cadastrarPetUseCase;
        private readonly ExcluirPetUseCase _excluirPetUseCase;
        private readonly IPetRepository _petRepository;

        public PetController(
            CadastrarPetUseCase cadastrarPetUseCase,
            ExcluirPetUseCase excluirPetUseCase,
            IPetRepository petRepository)
        {
            _cadastrarPetUseCase = cadastrarPetUseCase;
            _excluirPetUseCase = excluirPetUseCase;
            _petRepository = petRepository;
        }

        [HttpGet("ListarPets")]
        public IActionResult ListarPets()
        {
            var list = _petRepository.GetAll();
            return Ok(list);
        }

        [HttpPost("CadastrarPet")]
        public IActionResult CadastrarPet([FromBody] CreatePetDTO dto)
        {
            try
            {
                _cadastrarPetUseCase.Run(dto);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("ExcluirPet")]
        public IActionResult ExcluirPet([FromQuery] Guid id)
        {
            try
            {
                _excluirPetUseCase.Run(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
```

### ConsultaController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using VetClinic.DTOs.Requests;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;
using VetClinic.UseCases;

namespace VetClinic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : ControllerBase
    {
        private readonly AgendarConsultaUseCase _agendarUseCase;
        private readonly CancelarConsultaUseCase _cancelarUseCase;
        private readonly IConsultaService _service;
        private readonly IVeterinarioRepository _veterinarioRepository;

        public ConsultaController(
            AgendarConsultaUseCase agendarUseCase,
            CancelarConsultaUseCase cancelarUseCase,
            IConsultaService service,
            IVeterinarioRepository veterinarioRepository)
        {
            _agendarUseCase = agendarUseCase;
            _cancelarUseCase = cancelarUseCase;
            _service = service;
            _veterinarioRepository = veterinarioRepository;
        }

        [HttpGet("ListarConsultas")]
        public IActionResult ListarConsultas()
        {
            var list = _service.ListarConsultas();
            return Ok(list);
        }

        [HttpGet("ListarVeterinarios")]
        public IActionResult ListarVeterinarios()
        {
            var list = _veterinarioRepository.GetAll();
            return Ok(list);
        }

        [HttpPost("Agendar")]
        public IActionResult Agendar([FromBody] AgendarConsultaRequest request)
        {
            try
            {
                _agendarUseCase.Run(request);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Cancelar")]
        public IActionResult Cancelar([FromQuery] Guid id)
        {
            try
            {
                _cancelarUseCase.Run(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
```

---

## 11. DATA/CONTEXT — mesmo padrão do Context.cs do UniBet, mas SQLite

```csharp
using Microsoft.EntityFrameworkCore;
using VetClinic.Entities;

namespace VetClinic.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Dono> Donos { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dono>(entity =>
            {
                entity.HasKey(d => d.Id);
                // Cpf é ValueObject — mapear como owned type
                entity.OwnsOne(d => d.Cpf, cpf =>
                {
                    cpf.Property(c => c.Value).HasColumnName("Cpf");
                });
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.Dono)
                      .WithMany()
                      .HasForeignKey(p => p.DonoId);
            });

            modelBuilder.Entity<Veterinario>(entity =>
            {
                entity.HasKey(v => v.Id);
            });

            modelBuilder.Entity<Consulta>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Pet)
                      .WithMany()
                      .HasForeignKey(c => c.PetId);
                entity.HasOne(c => c.Veterinario)
                      .WithMany()
                      .HasForeignKey(c => c.VeterinarioId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
```

---

## 12. PROGRAM.CS — mesmo padrão do UniBet, trocando MySQL por SQLite

```csharp
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Interfaces.IRepositories;
using VetClinic.Interfaces.IServices;
using VetClinic.Repositories;
using VetClinic.Services;
using VetClinic.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS para o React (porta 5173 do Vite)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// SQLite (igual ao UniBet mas troca MySQL por SQLite)
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite("Data Source=vetclinic.db")
);

// Repositories
builder.Services.AddScoped<IDonoRepository, DonoRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();

// Services
builder.Services.AddScoped<IDonoService, DonoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

// Use Cases
builder.Services.AddScoped<CadastrarDonoUseCase>();
builder.Services.AddScoped<CadastrarPetUseCase>();
builder.Services.AddScoped<AgendarConsultaUseCase>();
builder.Services.AddScoped<CancelarConsultaUseCase>();
builder.Services.AddScoped<ExcluirPetUseCase>();

var app = builder.Build();

// Criar banco automaticamente (sem precisar rodar migration)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### VetClinic.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```

---

## 13. FRONTEND REACT — mesmo consumo de API, estilo simples

### api/api.js

```javascript
const BASE_URL = 'http://localhost:5000';

// ── DONOS ──────────────────────────────────────
export async function listarDonos() {
  const res = await fetch(`${BASE_URL}/Dono/ListarDonos`);
  return res.json();
}

export async function cadastrarDono(dto) {
  const res = await fetch(`${BASE_URL}/Dono/CadastrarDono`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  if (!res.ok) {
    const erro = await res.text();
    throw new Error(erro);
  }
}

// ── PETS ──────────────────────────────────────
export async function listarPets() {
  const res = await fetch(`${BASE_URL}/Pet/ListarPets`);
  return res.json();
}

export async function cadastrarPet(dto) {
  const res = await fetch(`${BASE_URL}/Pet/CadastrarPet`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  if (!res.ok) {
    const erro = await res.text();
    throw new Error(erro);
  }
}

export async function excluirPet(id) {
  const res = await fetch(`${BASE_URL}/Pet/ExcluirPet?id=${id}`, {
    method: 'DELETE',
  });
  if (!res.ok) {
    const erro = await res.text();
    throw new Error(erro);
  }
}

// ── CONSULTAS ─────────────────────────────────
export async function listarConsultas() {
  const res = await fetch(`${BASE_URL}/Consulta/ListarConsultas`);
  return res.json();
}

export async function listarVeterinarios() {
  const res = await fetch(`${BASE_URL}/Consulta/ListarVeterinarios`);
  return res.json();
}

export async function agendarConsulta(request) {
  const res = await fetch(`${BASE_URL}/Consulta/Agendar`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });
  if (!res.ok) {
    const erro = await res.text();
    throw new Error(erro);
  }
}

export async function cancelarConsulta(id) {
  const res = await fetch(`${BASE_URL}/Consulta/Cancelar?id=${id}`, {
    method: 'PATCH',
  });
  if (!res.ok) {
    const erro = await res.text();
    throw new Error(erro);
  }
}
```

---

## 14. REGRAS DE NEGÓCIO

| # | Regra | Onde fica |
|---|-------|-----------|
| RN01 | CPF não pode ser duplicado | `Cpf` (ValueObject valida formato) + `CadastrarDonoUseCase.Run()` verifica duplicata |
| RN02 | Pet deve ter dono válido | `CadastrarPetUseCase.Run()` busca dono antes de criar |
| RN03 | Sem conflito de horário do veterinário (±30min) | `ConsultaRepository.TemConflitoDeHorario()` + `AgendarConsultaUseCase.Run()` |
| RN04 | Não excluir pet com consulta futura | `PetRepository.TemConsultaFutura()` + `ExcluirPetUseCase.Run()` |
| RN05 | Data da consulta não pode ser passada | `Consulta.Validar()` dentro da entidade |
| RN06 | Só cancela consulta com status Agendada | `Consulta.Cancelar()` dentro da entidade |

---

## 15. INSTRUÇÕES PARA O CODEX

```
Você está implementando o VetClinic seguindo EXATAMENTE o padrão do projeto UniBet
do Professor William Castro. Analise os padrões da seção 1 antes de escrever qualquer código.

PADRÕES OBRIGATÓRIOS (baseados no UniBet):
  P01. EntityBase com Id (Guid), CreatedAt, RemovedAt — IGUAL ao UniBet
  P02. Entidades com métodos de negócio internos (Validar, Cancelar) — igual User.UpdatePassword()
  P03. ValueObjects com construtor que valida e lança Exception — igual Email e Amount
  P04. UseCases com método Run() — NÃO usar ExecutarAsync
  P05. UseCases com try/catch relançando Exception — igual PlaceGameUseCase
  P06. Controllers com [Route("[controller]")] — igual GameController e UserController
  P07. Controllers com try/catch retornando BadRequest(ex.Message)
  P08. Repositories recebem Context no construtor — igual UserRepository
  P09. Repositories com GetAll(), FindById(Guid), Create(), Update(), Delete()
  P10. Services implementam interface IXService — igual UserService : IUserService
  P11. Program.cs registra tudo com AddScoped — igual UniBet
  P12. EF Core com DbContext (Context.cs) — trocar MySQL por SQLite
  P13. db.Database.EnsureCreated() no startup — sem migration manual
  P14. CORS habilitado para http://localhost:5173

ORDEM DE IMPLEMENTAÇÃO:
  1. EntityBase.cs
  2. ValueObjects/Cpf.cs
  3. Entities/ (Dono, Pet, Veterinario, Consulta)
  4. DTOs/ e DTOs/Requests/
  5. Interfaces/IRepositories/ (4 interfaces)
  6. Interfaces/IServices/ (2 interfaces)
  7. Data/Context.cs
  8. Repositories/ (4 repositórios)
  9. Services/ (DonoService, ConsultaService)
  10. UseCases/ (5 use cases)
  11. Controllers/ (DonoController, PetController, ConsultaController)
  12. Program.cs + VetClinic.csproj
  13. Frontend: npm create vite@latest vetclinic-web -- --template react
  14. Frontend: src/api/api.js + páginas Donos, Pets, Consultas

SE FALTAR INFORMAÇÃO: sinalizar [SPEC INCOMPLETA: campo] e aguardar.
NUNCA usar padrão diferente do observado no UniBet sem justificativa.
```

---

*Spec VetClinic · UNIMAR · Trabalho Final · Professor William Castro*
*Padrão baseado no projeto UniBet — .NET 10 + EF Core + SQLite + React*
