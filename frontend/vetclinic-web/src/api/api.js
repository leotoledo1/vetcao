                                                                
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
                                                            
    const erro = await res.text()
    throw new Error(erro)
  }
}

export async function excluirPet(id) {
  const res = await fetch(`${BASE_URL}/Pet/ExcluirPet?id=${id}`, {
    method: 'DELETE',
  })

  if (!res.ok) {
                                                           
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
                                                                    
    const erro = await res.text()
    throw new Error(erro)
  }
}

export async function cancelarConsulta(id) {
  const res = await fetch(`${BASE_URL}/Consulta/Cancelar?id=${id}`, {
    method: 'PATCH',
  })

  if (!res.ok) {
                                                                          
    const erro = await res.text()
    throw new Error(erro)
  }
}
