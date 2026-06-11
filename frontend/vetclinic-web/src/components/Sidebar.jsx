import {
  CalendarDays,
  HeartPulse,
  LayoutDashboard,
  Menu,
  PawPrint,
  Users,
  X,
} from 'lucide-react'

                                                                                 
const iconMap = {
  dashboard: LayoutDashboard,
  donos: Users,
  pets: PawPrint,
  consultas: CalendarDays,
}

function Sidebar({ activeScreen, onChangeScreen, isOpen, onToggle }) {
  const items = [
    { id: 'dashboard', label: 'Dashboard' },
    { id: 'donos', label: 'Donos' },
    { id: 'pets', label: 'Pets' },
    { id: 'consultas', label: 'Consultas' },
  ]

  return (
    <>
      <button type="button" className="mobile-menu-button" onClick={onToggle} aria-label="Abrir menu">
        <Menu size={20} />
      </button>
      <aside className={isOpen ? 'sidebar sidebar-open' : 'sidebar'}>
        <div className="sidebar-header">
          <div className="brand-mark">
            <HeartPulse size={18} />
          </div>
          <div>
            <strong>VetClinic</strong>
            <p>Clinica veterinaria</p>
          </div>
          <button type="button" className="sidebar-close" onClick={onToggle} aria-label="Fechar menu">
            <X size={18} />
          </button>
        </div>

        <nav className="sidebar-nav" aria-label="Menu lateral">
          {items.map((item) => {
            const Icon = iconMap[item.id]
            const isActive = activeScreen === item.id

            return (
              <button
                key={item.id}
                type="button"
                className={isActive ? 'sidebar-link active' : 'sidebar-link'}
                onClick={() => {
                  onChangeScreen(item.id)
                  onToggle(false)
                }}
              >
                <Icon size={18} />
                <span>{item.label}</span>
              </button>
            )
          })}
        </nav>

        <div className="sidebar-footer">
          <p>Frontend web em React + Vite</p>
        </div>
      </aside>
      {isOpen ? <button type="button" className="sidebar-backdrop" onClick={() => onToggle(false)} /> : null}
    </>
  )
}

export default Sidebar
