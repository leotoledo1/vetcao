# VetClinic

Sistema de gerenciamento de consultas veterinárias desenvolvido com .NET no backend e React + Vite no frontend.

## Sobre o projeto

O VetClinic foi desenvolvido com o objetivo de aplicar conceitos vistos em sala de aula, principalmente organização em camadas, arquitetura de software e Domain-Driven Design (DDD).

O sistema permite o cadastro de donos e pets, além do agendamento e gerenciamento de consultas veterinárias.

## Tecnologias utilizadas

### Backend

* ASP.NET Core
* Entity Framework Core
* SQLite

### Frontend

* React
* Vite
* JavaScript
* CSS

## Estrutura do projeto

O projeto foi dividido em dois contextos principais:

### Cadastro

Responsável pelo gerenciamento de:

* Donos
* Pets

### Consultas

Responsável pelo gerenciamento de:

* Agendamento de consultas
* Cancelamento de consultas

## Funcionalidades

* Cadastro de donos
* Cadastro de pets
* Listagem de donos, pets e consultas
* Agendamento de consultas
* Cancelamento de consultas
* Exclusão de pets

## Regras de negócio implementadas

Algumas regras foram implementadas para garantir a consistência dos dados:

* Não permite cadastrar dois donos com o mesmo CPF;
* Não permite cadastrar donos com CPF inválido;
* Não permite agendar consultas em horários já ocupados pelo mesmo veterinário;
* Não permite excluir um pet que possua consultas futuras agendadas;
* Não permite cadastrar datas de nascimento inválidas;
* Apenas consultas futuras podem ser canceladas.

## Organização do backend

O projeto foi organizado em camadas para facilitar a manutenção e separação de responsabilidades.

### Controllers

Responsáveis por receber as requisições da API.

### Use Cases

Contêm a lógica de aplicação do sistema.

Exemplos:

* CadastrarDonoUseCase
* CadastrarPetUseCase
* AgendarConsultaUseCase
* CancelarConsultaUseCase
* ExcluirPetUseCase

### Domain

Contém as entidades, interfaces e regras de negócio.

### Infrastructure

Responsável pelo acesso ao banco de dados e implementação dos repositórios.

## Como executar o backend

Abra um terminal na pasta `VetClinic-Api` e execute:

```powershell
dotnet run
```

Após iniciar, a API ficará disponível em:

```text
http://localhost:5000
```

### Principais endpoints

#### Donos

* GET /Dono/ListarDonos
* POST /Dono/CadastrarDono

#### Pets

* GET /Pet/ListarPets
* POST /Pet/CadastrarPet
* DELETE /Pet/ExcluirPet?id={id}

#### Consultas

* GET /Consulta/ListarConsultas
* POST /Consulta/Agendar
* PATCH /Consulta/Cancelar?id={id}

## Como executar o frontend

Abra um terminal na pasta `frontend/vetclinic-web` e execute:

```powershell
npm install
npm run dev
```

A aplicação estará disponível em:

```text
http://localhost:5173
```

## Observações

* O banco de dados utilizado é o SQLite;
* O banco é criado automaticamente na primeira execução;
* O backend possui configuração de CORS para comunicação com o frontend;
* A exclusão de pets é realizada de forma lógica através do campo `RemovedAt`.

## Considerações finais

O projeto foi desenvolvido com foco na aplicação dos conceitos de DDD, separação de responsabilidades e boas práticas de desenvolvimento. Além de atender aos requisitos propostos, buscou-se manter uma estrutura simples, organizada e de fácil manutenção.
