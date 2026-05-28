// Centraliza todas as chamadas HTTP que o front faz para a API.
const BASE_URL = 'http://localhost:5000'

export async function listarDonos() {
  const res = await fetch(`${BASE_URL}/Dono/ListarDonos`)
  return res.json()
}

export async function cadastrarDono(dto) {
  const res = await fetch(`${BASE_URL}/Dono/CadastrarDono`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  })

  if (!res.ok) {
    // Se o back responder com erro, repassa a mensagem para a tela.
    const erro = await res.text()
    throw new Error(erro)
  }
}

export async function listarPets() {
  const res = await fetch(`${BASE_URL}/Pet/ListarPets`)
  return res.json()
}

export async function cadastrarPet(dto) {
  const res = await fetch(`${BASE_URL}/Pet/CadastrarPet`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  })

  if (!res.ok) {
    // Mantem a mesma regra de erro para o cadastro de pets.
    const erro = await res.text()
    throw new Error(erro)
  }
}

export async function excluirPet(id) {
  const res = await fetch(`${BASE_URL}/Pet/ExcluirPet?id=${id}`, {
    method: 'DELETE',
  })

  if (!res.ok) {
    // Retorna o erro do servidor para o componente tratar.
    const erro = await res.text()
    throw new Error(erro)
  }
}

export async function listarConsultas() {
  const res = await fetch(`${BASE_URL}/Consulta/ListarConsultas`)
  return res.json()
}

export async function listarVeterinarios() {
  const res = await fetch(`${BASE_URL}/Consulta/ListarVeterinarios`)
  return res.json()
}

export async function agendarConsulta(request) {
  const res = await fetch(`${BASE_URL}/Consulta/Agendar`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  })

  if (!res.ok) {
    // O front mostra a mensagem do back quando o agendamento falha.
    const erro = await res.text()
    throw new Error(erro)
  }
}

export async function cancelarConsulta(id) {
  const res = await fetch(`${BASE_URL}/Consulta/Cancelar?id=${id}`, {
    method: 'PATCH',
  })

  if (!res.ok) {
    // Em caso de falha no cancelamento, a tela recebe a mensagem do back.
    const erro = await res.text()
    throw new Error(erro)
  }
}
