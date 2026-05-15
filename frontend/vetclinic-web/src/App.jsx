import { startTransition, useDeferredValue, useEffect, useState } from 'react'
import {
  CalendarDays,
  CircleAlert,
  PawPrint,
  Stethoscope,
  Users,
} from 'lucide-react'
import './App.css'
import {
  agendarConsulta,
  cadastrarDono,
  cadastrarPet,
  cancelarConsulta,
  excluirPet,
  listarConsultas,
  listarDonos,
  listarPets,
  listarVeterinarios,
} from './api/api'
import DashboardCard from './components/DashboardCard'
import Loading from './components/Loading'
import Navbar from './components/Navbar'
import Sidebar from './components/Sidebar'
import StatusBadge from './components/StatusBadge'

const screenMeta = {
  dashboard: {
    title: 'Dashboard',
    subtitle: 'Resumo rapido da clinica, com acesso direto aos principais modulos.',
  },
  donos: {
    title: 'Donos',
    subtitle: 'Cadastre responsaveis, acompanhe contatos e encontre registros com rapidez.',
  },
  pets: {
    title: 'Pets',
    subtitle: 'Organize os animais cadastrados em um layout mais visual e facil de apresentar.',
  },
  consultas: {
    title: 'Consultas',
    subtitle: 'Agende consultas, acompanhe status e mantenha a agenda sob controle.',
  },
}

function App() {
  const [activeScreen, setActiveScreen] = useState('dashboard')
  const [isSidebarOpen, setIsSidebarOpen] = useState(false)
  const [theme, setTheme] = useState('light')
  const [donos, setDonos] = useState([])
  const [pets, setPets] = useState([])
  const [consultas, setConsultas] = useState([])
  const [veterinarios, setVeterinarios] = useState([])
  const [mensagem, setMensagem] = useState('')
  const [erro, setErro] = useState('')
  const [isLoading, setIsLoading] = useState(true)
  const [isRefreshing, setIsRefreshing] = useState(false)
  const [searchDono, setSearchDono] = useState('')

  const [donoForm, setDonoForm] = useState({
    nome: '',
    cpf: '',
    telefone: '',
    email: '',
  })

  const [petForm, setPetForm] = useState({
    nome: '',
    especie: '',
    raca: '',
    dataNascimento: '',
    donoId: '',
  })

  const [consultaForm, setConsultaForm] = useState({
    petId: '',
    veterinarioId: '',
    dataHora: '',
    motivo: '',
  })

  const deferredSearchDono = useDeferredValue(searchDono)

  const term = deferredSearchDono.trim().toLowerCase()
  const filteredDonos = !term
    ? donos
    : donos.filter((dono) => {
        return (
          dono.nome?.toLowerCase().includes(term) ||
          dono.email?.toLowerCase().includes(term) ||
          dono.telefone?.toLowerCase().includes(term) ||
          dono.cpf?.value?.includes(term)
        )
      })

  const proximaConsulta = consultas.find((consulta) => consulta.status === 'Agendada') || null

  useEffect(() => {
    const savedTheme = window.localStorage.getItem('vetclinic-theme')
    if (savedTheme === 'dark' || savedTheme === 'light') {
      setTheme(savedTheme)
    }
  }, [])

  useEffect(() => {
    document.documentElement.setAttribute('data-theme', theme)
    window.localStorage.setItem('vetclinic-theme', theme)
  }, [theme])

  useEffect(() => {
    carregarTudo({ initial: true })
  }, [])

  async function carregarTudo({ initial = false } = {}) {
    if (initial) {
      setIsLoading(true)
    } else {
      setIsRefreshing(true)
    }

    try {
      const [listaDonos, listaPets, listaConsultas, listaVeterinarios] = await Promise.all([
        listarDonos(),
        listarPets(),
        listarConsultas(),
        listarVeterinarios(),
      ])

      setDonos(listaDonos)
      setPets(listaPets)
      setConsultas(listaConsultas)
      setVeterinarios(listaVeterinarios)
    } catch {
      setErro('Nao foi possivel carregar os dados iniciais da aplicacao.')
    } finally {
      setIsLoading(false)
      setIsRefreshing(false)
    }
  }

  function limparMensagens() {
    setMensagem('')
    setErro('')
  }

  async function recarregarDonos() {
    setDonos(await listarDonos())
  }

  async function recarregarPets() {
    setPets(await listarPets())
  }

  async function recarregarConsultas() {
    setConsultas(await listarConsultas())
  }

  async function onCadastrarDono(event) {
    event.preventDefault()
    limparMensagens()

    try {
      await cadastrarDono(donoForm)
      setMensagem('Dono cadastrado com sucesso.')
      setDonoForm({
        nome: '',
        cpf: '',
        telefone: '',
        email: '',
      })
      await recarregarDonos()
    } catch (error) {
      setErro(error.message)
    }
  }

  async function onCadastrarPet(event) {
    event.preventDefault()
    limparMensagens()

    try {
      await cadastrarPet(petForm)
      setMensagem('Pet cadastrado com sucesso.')
      setPetForm({
        nome: '',
        especie: '',
        raca: '',
        dataNascimento: '',
        donoId: '',
      })
      await recarregarPets()
    } catch (error) {
      setErro(error.message)
    }
  }

  async function onAgendarConsulta(event) {
    event.preventDefault()
    limparMensagens()

    try {
      await agendarConsulta(consultaForm)
      setMensagem('Consulta agendada com sucesso.')
      setConsultaForm({
        petId: '',
        veterinarioId: '',
        dataHora: '',
        motivo: '',
      })
      await recarregarConsultas()
    } catch (error) {
      setErro(error.message)
    }
  }

  async function onExcluirPet(id, nome) {
    const confirmed = window.confirm(`Deseja realmente excluir o pet ${nome}?`)
    if (!confirmed) {
      return
    }

    limparMensagens()

    try {
      await excluirPet(id)
      setMensagem('Pet excluido com sucesso.')
      await recarregarPets()
      await recarregarConsultas()
    } catch (error) {
      setErro(error.message)
    }
  }

  async function onCancelarConsulta(id) {
    limparMensagens()

    try {
      await cancelarConsulta(id)
      setMensagem('Consulta cancelada com sucesso.')
      await recarregarConsultas()
    } catch (error) {
      setErro(error.message)
    }
  }

  function formatarData(data) {
    if (!data) {
      return '-'
    }

    return new Date(data).toLocaleString('pt-BR')
  }

  function formatarDataSimples(data) {
    if (!data) {
      return '-'
    }

    return new Date(data).toLocaleDateString('pt-BR')
  }

  function goToScreen(screen) {
    startTransition(() => {
      setActiveScreen(screen)
    })
  }

  function toggleTheme() {
    setTheme((currentTheme) => (currentTheme === 'dark' ? 'light' : 'dark'))
  }

  const currentMeta = screenMeta[activeScreen]

  return (
    <div className="layout-shell">
      <Sidebar
        activeScreen={activeScreen}
        onChangeScreen={goToScreen}
        isOpen={isSidebarOpen}
        onToggle={(nextState) =>
          setIsSidebarOpen((currentState) =>
            typeof nextState === 'boolean' ? nextState : !currentState,
          )
        }
      />

      <div className="layout-content">
        <Navbar
          title={currentMeta.title}
          subtitle={currentMeta.subtitle}
          theme={theme}
          onToggleTheme={toggleTheme}
        />

        {mensagem ? <div className="banner success">{mensagem}</div> : null}
        {erro ? <div className="banner error">{erro}</div> : null}
        {isRefreshing ? <Loading label="Atualizando informacoes..." /> : null}

        {isLoading ? (
          <section className="page-section">
            <Loading />
          </section>
        ) : null}

        {!isLoading && activeScreen === 'dashboard' ? (
          <section className="page-section dashboard-flow">
            <div className="dashboard-hero">
              <div>
                <p className="screen-eyebrow">Visao geral</p>
                <h2>Uma interface mais organizada para apresentar o VetClinic.</h2>
                <p className="hero-paragraph">
                  Sidebar fixa, cards com leitura rapida e modulos separados para deixar o
                  sistema mais leve e profissional no navegador.
                </p>
              </div>
              <div className="dashboard-callout">
                <span>Proxima consulta</span>
                <strong>
                  {proximaConsulta ? formatarData(proximaConsulta.dataHora) : 'Nenhuma agenda ativa'}
                </strong>
                <p>
                  {proximaConsulta
                    ? `${proximaConsulta.pet?.nome || 'Pet'} com ${proximaConsulta.veterinario?.nome || 'veterinario'}`
                    : 'Assim que uma consulta for agendada, ela aparece aqui.'}
                </p>
              </div>
            </div>

            <div className="dashboard-grid">
              <DashboardCard
                icon={Users}
                label="Total de donos"
                value={donos.length}
                helper="Responsaveis cadastrados"
                accent="green"
              />
              <DashboardCard
                icon={PawPrint}
                label="Total de pets"
                value={pets.length}
                helper="Animais ativos na base"
                accent="mint"
              />
              <DashboardCard
                icon={CalendarDays}
                label="Total de consultas"
                value={consultas.length}
                helper="Historico de atendimentos"
                accent="lime"
              />
              <DashboardCard
                icon={Stethoscope}
                label="Veterinarios"
                value={veterinarios.length}
                helper="Disponiveis para agendamento"
                accent="forest"
              />
            </div>

            <div className="dashboard-panels">
              <article className="surface-card">
                <div className="section-header">
                  <div>
                    <p className="section-label">Atalhos</p>
                    <h3>Fluxos principais</h3>
                  </div>
                </div>
                <div className="quick-actions">
                  <button type="button" className="action-tile" onClick={() => goToScreen('donos')}>
                    <Users size={18} />
                    <span>Cadastrar dono</span>
                  </button>
                  <button type="button" className="action-tile" onClick={() => goToScreen('pets')}>
                    <PawPrint size={18} />
                    <span>Cadastrar pet</span>
                  </button>
                  <button
                    type="button"
                    className="action-tile"
                    onClick={() => goToScreen('consultas')}
                  >
                    <CalendarDays size={18} />
                    <span>Agendar consulta</span>
                  </button>
                </div>
              </article>

              <article className="surface-card">
                <div className="section-header">
                  <div>
                    <p className="section-label">Resumo</p>
                    <h3>Agenda em destaque</h3>
                  </div>
                </div>
                <div className="consultation-preview">
                  {consultas.length === 0 ? (
                    <p className="empty-copy">Nenhuma consulta registrada ainda.</p>
                  ) : (
                    consultas.slice(0, 3).map((consulta) => (
                      <div className="preview-row" key={consulta.id}>
                        <div>
                          <strong>{consulta.pet?.nome || 'Pet nao informado'}</strong>
                          <p>{consulta.veterinario?.nome || 'Veterinario nao informado'}</p>
                        </div>
                        <StatusBadge status={consulta.status} />
                      </div>
                    ))
                  )}
                </div>
              </article>
            </div>
          </section>
        ) : null}

        {!isLoading && activeScreen === 'donos' ? (
          <section className="page-section screen-stack">
            <div className="screen-grid two-columns">
              <article className="surface-card">
                <div className="section-header">
                  <div>
                    <p className="section-label">Formulario</p>
                    <h3>Novo dono</h3>
                  </div>
                </div>

                <form className="form-grid" onSubmit={onCadastrarDono}>
                  <label>
                    <span>Nome</span>
                    <input
                      value={donoForm.nome}
                      onChange={(event) =>
                        setDonoForm({ ...donoForm, nome: event.target.value })
                      }
                      placeholder="Ex.: Mariana Costa"
                      required
                    />
                  </label>
                  <label>
                    <span>CPF</span>
                    <input
                      value={donoForm.cpf}
                      onChange={(event) =>
                        setDonoForm({ ...donoForm, cpf: event.target.value })
                      }
                      placeholder="Somente numeros ou formatado"
                      required
                    />
                  </label>
                  <label>
                    <span>Telefone</span>
                    <input
                      value={donoForm.telefone}
                      onChange={(event) =>
                        setDonoForm({ ...donoForm, telefone: event.target.value })
                      }
                      placeholder="(11) 99999-9999"
                      required
                    />
                  </label>
                  <label>
                    <span>Email</span>
                    <input
                      type="email"
                      value={donoForm.email}
                      onChange={(event) =>
                        setDonoForm({ ...donoForm, email: event.target.value })
                      }
                      placeholder="email@exemplo.com"
                      required
                    />
                  </label>
                  <button type="submit" className="primary-button">
                    Cadastrar dono
                  </button>
                </form>
              </article>

              <article className="surface-card">
                <div className="section-header align-end">
                  <div>
                    <p className="section-label">Lista</p>
                    <h3>Donos cadastrados</h3>
                  </div>
                  <label className="search-field">
                    <CircleAlert size={16} />
                    <input
                      value={searchDono}
                      onChange={(event) => setSearchDono(event.target.value)}
                      placeholder="Buscar por nome, email, telefone ou CPF"
                    />
                  </label>
                </div>

                <div className="table-shell">
                  <table className="owners-table">
                    <thead>
                      <tr>
                        <th>Nome</th>
                        <th>CPF</th>
                        <th>Telefone</th>
                        <th>Email</th>
                      </tr>
                    </thead>
                    <tbody>
                      {filteredDonos.length === 0 ? (
                        <tr>
                          <td colSpan="4" className="table-empty">
                            Nenhum dono encontrado.
                          </td>
                        </tr>
                      ) : (
                        filteredDonos.map((dono) => (
                          <tr key={dono.id}>
                            <td>{dono.nome}</td>
                            <td>{dono.cpf?.value}</td>
                            <td>{dono.telefone}</td>
                            <td>{dono.email}</td>
                          </tr>
                        ))
                      )}
                    </tbody>
                  </table>
                </div>
              </article>
            </div>
          </section>
        ) : null}

        {!isLoading && activeScreen === 'pets' ? (
          <section className="page-section screen-stack">
            <div className="screen-grid two-columns">
              <article className="surface-card">
                <div className="section-header">
                  <div>
                    <p className="section-label">Cadastro</p>
                    <h3>Novo pet</h3>
                  </div>
                </div>

                <form className="form-grid" onSubmit={onCadastrarPet}>
                  <label>
                    <span>Nome</span>
                    <input
                      value={petForm.nome}
                      onChange={(event) => setPetForm({ ...petForm, nome: event.target.value })}
                      placeholder="Ex.: Thor"
                      required
                    />
                  </label>
                  <label>
                    <span>Especie</span>
                    <input
                      value={petForm.especie}
                      onChange={(event) =>
                        setPetForm({ ...petForm, especie: event.target.value })
                      }
                      placeholder="Ex.: Cachorro"
                      required
                    />
                  </label>
                  <label>
                    <span>Raca</span>
                    <input
                      value={petForm.raca}
                      onChange={(event) => setPetForm({ ...petForm, raca: event.target.value })}
                      placeholder="Ex.: Golden Retriever"
                      required
                    />
                  </label>
                  <label>
                    <span>Data de nascimento</span>
                    <input
                      type="date"
                      value={petForm.dataNascimento}
                      onChange={(event) =>
                        setPetForm({ ...petForm, dataNascimento: event.target.value })
                      }
                      required
                    />
                  </label>
                  <label>
                    <span>Dono</span>
                    <select
                      value={petForm.donoId}
                      onChange={(event) => setPetForm({ ...petForm, donoId: event.target.value })}
                      required
                    >
                      <option value="">Selecione um dono</option>
                      {donos.map((dono) => (
                        <option key={dono.id} value={dono.id}>
                          {dono.nome}
                        </option>
                      ))}
                    </select>
                  </label>
                  <button type="submit" className="primary-button">
                    Cadastrar pet
                  </button>
                </form>
              </article>

              <article className="surface-card">
                <div className="section-header">
                  <div>
                    <p className="section-label">Cards</p>
                    <h3>Pets cadastrados</h3>
                  </div>
                </div>

                <div className="pets-grid">
                  {pets.length === 0 ? <p className="empty-copy">Nenhum pet cadastrado.</p> : null}
                  {pets.map((pet) => (
                    <article className="pet-card" key={pet.id}>
                      <div className="pet-avatar">
                        <PawPrint size={18} />
                      </div>
                      <div className="pet-card-header">
                        <div>
                          <strong>{pet.nome}</strong>
                          <p>{pet.especie} | {pet.raca}</p>
                        </div>
                        <span className="pet-date">{formatarDataSimples(pet.dataNascimento)}</span>
                      </div>
                      <p className="pet-owner">Dono: {pet.dono?.nome || 'Nao informado'}</p>
                      <div className="pet-actions">
                        <button
                          type="button"
                          className="secondary-button"
                          onClick={() => onExcluirPet(pet.id, pet.nome)}
                        >
                          Excluir
                        </button>
                      </div>
                    </article>
                  ))}
                </div>
              </article>
            </div>
          </section>
        ) : null}

        {!isLoading && activeScreen === 'consultas' ? (
          <section className="page-section screen-stack">
            <div className="screen-grid two-columns">
              <article className="surface-card">
                <div className="section-header">
                  <div>
                    <p className="section-label">Agenda</p>
                    <h3>Agendar consulta</h3>
                  </div>
                </div>

                <form className="form-grid" onSubmit={onAgendarConsulta}>
                  <label>
                    <span>Pet</span>
                    <select
                      value={consultaForm.petId}
                      onChange={(event) =>
                        setConsultaForm({ ...consultaForm, petId: event.target.value })
                      }
                      required
                    >
                      <option value="">Selecione um pet</option>
                      {pets.map((pet) => (
                        <option key={pet.id} value={pet.id}>
                          {pet.nome}
                        </option>
                      ))}
                    </select>
                  </label>
                  <label>
                    <span>Veterinario</span>
                    <select
                      value={consultaForm.veterinarioId}
                      onChange={(event) =>
                        setConsultaForm({ ...consultaForm, veterinarioId: event.target.value })
                      }
                      required
                    >
                      <option value="">Selecione um veterinario</option>
                      {veterinarios.map((veterinario) => (
                        <option key={veterinario.id} value={veterinario.id}>
                          {veterinario.nome} | {veterinario.especialidade}
                        </option>
                      ))}
                    </select>
                  </label>
                  <label>
                    <span>Data e hora</span>
                    <input
                      type="datetime-local"
                      value={consultaForm.dataHora}
                      onChange={(event) =>
                        setConsultaForm({ ...consultaForm, dataHora: event.target.value })
                      }
                      required
                    />
                  </label>
                  <label>
                    <span>Motivo</span>
                    <textarea
                      value={consultaForm.motivo}
                      onChange={(event) =>
                        setConsultaForm({ ...consultaForm, motivo: event.target.value })
                      }
                      rows="4"
                      placeholder="Descreva rapidamente o motivo da consulta"
                      required
                    />
                  </label>
                  <button type="submit" className="primary-button">
                    Agendar consulta
                  </button>
                </form>
              </article>

              <article className="surface-card">
                <div className="section-header">
                  <div>
                    <p className="section-label">Historico</p>
                    <h3>Consultas registradas</h3>
                  </div>
                </div>

                <div className="consultation-list">
                  {consultas.length === 0 ? (
                    <p className="empty-copy">Nenhuma consulta cadastrada.</p>
                  ) : null}
                  {consultas.map((consulta) => (
                    <article className="consultation-card" key={consulta.id}>
                      <div className="consultation-card-top">
                        <div>
                          <strong>{consulta.pet?.nome || 'Pet nao informado'}</strong>
                          <p>{consulta.veterinario?.nome || 'Veterinario nao informado'}</p>
                        </div>
                        <StatusBadge status={consulta.status} />
                      </div>
                      <div className="consultation-meta">
                        <span>Data: {formatarData(consulta.dataHora)}</span>
                        <span>Motivo: {consulta.motivo}</span>
                      </div>
                      {consulta.status === 'Agendada' ? (
                        <div className="consultation-actions">
                          <button
                            type="button"
                            className="secondary-button"
                            onClick={() => onCancelarConsulta(consulta.id)}
                          >
                            Cancelar consulta
                          </button>
                        </div>
                      ) : null}
                    </article>
                  ))}
                </div>
              </article>
            </div>
          </section>
        ) : null}
      </div>
    </div>
  )
}

export default App
