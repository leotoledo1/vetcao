import { Moon, Sun } from 'lucide-react'

// Barra superior com titulo da tela e alternancia de tema.
function Navbar({ title, subtitle, theme, onToggleTheme }) {
  return (
    <header className="topbar">
      <div>
        <p className="screen-eyebrow">VetClinic</p>
        <h1 className="screen-title">{title}</h1>
        <p className="screen-subtitle">{subtitle}</p>
      </div>

      <button type="button" className="theme-button" onClick={onToggleTheme}>
        {theme === 'dark' ? <Sun size={18} /> : <Moon size={18} />}
        <span>{theme === 'dark' ? 'Modo claro' : 'Modo escuro'}</span>
      </button>
    </header>
  )
}

export default Navbar
